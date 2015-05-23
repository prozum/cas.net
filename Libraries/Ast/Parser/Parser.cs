﻿using System;
using System.Collections.Generic;

namespace Ast
{

    public enum ParseContext
    {
        List,
        ScopeSingle,
        ScopeMulti,
        ScopeGlobal,
        Colon,
        Parenthesis
    }

    public class Parser
    {
        Stack<Scope> scopeStack = new Stack<Scope>();
        Stack<ParseContext> contextStack = new Stack<ParseContext>();

        Stack<Queue<Expression>> exprStack = new Stack<Queue<Expression>>();
        Stack<Queue<UnaryOperator>> unaryStack = new Stack<Queue<UnaryOperator>>();
        Stack<Queue<BinaryOperator>> binaryStack = new Stack<Queue<BinaryOperator>>();

        Scope curScope { get { return scopeStack.Peek(); } }
        ParseContext curContext { get { return contextStack.Peek(); } }

        Queue<Expression> curExprStack { get { return exprStack.Peek(); } }
        Queue<UnaryOperator> curUnaryStack { get { return unaryStack.Peek(); } }
        Queue<BinaryOperator> curBinaryStack { get { return binaryStack.Peek(); } }

        readonly Token EOS = new Token(TokenKind.END_OF_STRING, "END_OF_STRING", new Pos());

        Queue<Token> tokens;
        Token curToken { get { return tokens.Count > 0 ? tokens.Peek() : EOS; } }
        bool Error { get { return curScope.Errors.Count > 0; } }
        bool expectUnary = true;

        public void Parse(string parseString)
        {
            Parse(parseString, new Scope());
        }

        public void Parse(string parseString, Scope global)
        {
            global.Errors.Clear();

            tokens = Scanner.Tokenize(parseString, global.Errors);
            if (global.Errors.Count > 0)
                return;

            ParseScope(false, global);

            if (global.Errors.Count > 0)
                Clear();
        }

        public void Clear()
        {
            scopeStack.Clear();
            contextStack.Clear();
            exprStack.Clear();
            unaryStack.Clear();
            binaryStack.Clear();
        }

        public Scope ParseScope(bool share = false, Scope global = null)
        {
            ParseContext cx;

            while (Eat(TokenKind.NEW_LINE));

            if (global != null)
            {
                cx = ParseContext.ScopeGlobal;
                scopeStack.Push(global);
            }
            else if (Eat(TokenKind.CURLY_START))
            {
                cx = ParseContext.ScopeMulti;
                scopeStack.Push(new Scope(curScope, share));
            }
            else
            {
                cx = ParseContext.ScopeSingle;
                scopeStack.Push(new Scope(curScope, share));
            }

            contextStack.Push(cx);
            ParseStatements();
            contextStack.Pop();

            if (Error)
                return scopeStack.Pop();

            if (cx == ParseContext.ScopeMulti && !Eat(TokenKind.CURLY_END))
                ReportError("Missing } bracket");
               
            return scopeStack.Pop();
        }

        public void ParseStatements()
        {
            while (tokens.Count > 0)
            {
                switch (curToken.Kind)
                {
                    case TokenKind.IF:
                        Eat();
                        curScope.Expressions.Add(ParseIfStmt());
                        break;
                    case TokenKind.FOR:
                        Eat();
                        curScope.Expressions.Add(ParseForStmt());
                        break;
                    case TokenKind.WHILE:
                        Eat();
                        curScope.Expressions.Add(ParseWhileStmt());
                        break;
                    case TokenKind.RET:
                        Eat();
                        curScope.Expressions.Add(new RetExpr(ParseExpr(), curScope));
                        break;

                    case TokenKind.ELIF:
                    case TokenKind.ELSE:
                    case TokenKind.CURLY_END:
                    case TokenKind.END_OF_STRING:
                        return;

                    case TokenKind.SEMICOLON:
                    case TokenKind.NEW_LINE:
                        Eat();
                        if (curContext == ParseContext.ScopeSingle)
                            return;
                        break;

                    default:
                        curScope.Expressions.Add(ParseExpr());
                        break;
                }

                if (Error)
                {
                    curScope.Expressions.Clear();
                    return;
                }
            }

            throw new Exception("ParseScope failed");
        }

        #region Peek Eat

        public bool Peek()
        {
            return tokens.Count > 0;
        }

        public bool Peek(TokenKind kind)
        {
            return curToken.Kind == kind;
        }

        public bool Eat()
        {
            if (tokens.Count > 0)
                tokens.Dequeue();

            return tokens.Count > 0;
        }

        public bool Eat(TokenKind kind)
        {
            if (tokens.Count > 0 && tokens.Peek().Kind == kind)
            {
                tokens.Dequeue();
                return true;
            }
                
            return false;
        }

        #endregion

        public IfExpr ParseIfStmt()
        {
            Expression cond;
            Expression expr;

            var stmt = new IfExpr(curScope);

            cond = ParseColon();
            if (Error)
                return null;
            stmt.Conditions.Add(cond);

            expr = ParseScope(true);
            if (Error)
                return null;
            stmt.Expressions.Add(expr);

            while (Eat(TokenKind.NEW_LINE));
            while (Eat(TokenKind.ELIF))
            {
                cond = ParseColon();
                if (Error)
                    return null;
                stmt.Conditions.Add(cond);

                expr = ParseScope(true);
                if (Error)
                    return null;
                stmt.Expressions.Add(expr);

                while (Eat(TokenKind.NEW_LINE));
            }

            while (Eat(TokenKind.NEW_LINE));
            if (Eat(TokenKind.ELSE))
            {
                Eat(TokenKind.COLON); // Optional colon

                expr = ParseScope(true);
                if (Error)
                    return null;
                stmt.Expressions.Add(expr);
            }

            return stmt;
        }

        public ForExpr ParseForStmt()
        {
            var stmt = new ForExpr(curScope);

            if (Peek(TokenKind.IDENTIFIER))
            {
                stmt.Var = curToken.Value;
                Eat();
            }
            else
            {
                ReportError("For: Missing symbol");
                return null;
            }

            if (!Eat(TokenKind.IN))
            {
                ReportError("For: Missing in");
                return null;
            }

            var list = ParseColon().Evaluate();

            if (Error)
                return null;

            if (list is List)
                stmt.List = list as List;
            else
            {
                ReportError("For: " + list + " is not a list");
                return null;
            }

            stmt.ForScope = ParseScope();

            return stmt;
        }

        public WhileExpr ParseWhileStmt()
        {
            Expression cond;
            Scope scope;

            var stmt = new WhileExpr(curScope);

            cond = ParseColon();
            if (Error)
                return null;
            stmt.Condition = cond;

            scope = ParseScope();
            if (Error)
                return null;
            stmt.Expression = scope;
            stmt.Condition.CurScope = scope;

            return stmt;
        }

        public Expression ParseColon()
        {
            contextStack.Push(ParseContext.Colon);
            var res = ParseExpr();
            contextStack.Pop();

            if (!Eat(TokenKind.COLON))
                ReportError("Missing :");

            return res;
        }

        public List ParseList()
        {
            var list = new List();

            Eat();
            contextStack.Push(ParseContext.List);

            while (tokens.Count > 0)
            {
                if (!(Eat(TokenKind.COMMA) || Eat(TokenKind.NEW_LINE)))
                    list.items.Add(ParseExpr());

                if (Error)
                    return list;

                if (Eat(TokenKind.SQUARE_END))
                    break;

                if (Peek(TokenKind.END_OF_STRING))
                {
                    ReportError("Missing ] bracket");
                    break;
                }
            }

            contextStack.Pop();

            if (list.items.Count == 1 && list.items[0] is Null)
                list.items.Clear();

            return list;
        }

        public Expression ParseParenthesis()
        {
        
            Eat();
            contextStack.Push(ParseContext.Parenthesis);

            Expression parent = ParseExpr();

            if (Error)
                return parent;

            if (!Eat(TokenKind.PARENT_END))
                ReportError("Missing ) bracket");

            contextStack.Pop();

            return parent;
        }

        public void ParseComment()
        {
            do
            {
                Eat();
            }
            while (curToken.Kind != TokenKind.NEW_LINE && curToken.Kind != TokenKind.END_OF_STRING);
        }

        public Expression ParseExpr()
        {
            bool done = false;
            bool eat;

            exprStack.Push(new Queue<Expression>());
            unaryStack.Push(new Queue<UnaryOperator>());
            binaryStack.Push(new Queue<BinaryOperator>());

            expectUnary = true;

            while (!done)
            {
                eat = true;

                switch (curToken.Kind)
                {
                    case TokenKind.END_OF_STRING:
                        done = true;
                        eat = false;
                        break;

                    case TokenKind.SEMICOLON:
                        if (curContext == ParseContext.ScopeGlobal || curContext == ParseContext.ScopeMulti)
                            done = true;
                        else if (curContext == ParseContext.ScopeSingle)
                        {
                            done = true;
                            eat = false;
                        }
                        else
                            ReportError("Unexpected ';' in " + curContext);
                        break;

                    case TokenKind.COLON:
                        if (curContext == ParseContext.Colon)
                        {
                            done = true;
                            eat = false;
                        }
                        else
                            ReportError("Unexpected ':' in " + curContext);
                        break;
                    
                    case TokenKind.COMMA:
                        if (curContext == ParseContext.List)
                            done = true;
                        else
                            ReportError("Unexpected ',' in " + curContext);
                        break;

                    case TokenKind.NEW_LINE:
                        if (curContext == ParseContext.ScopeSingle)
                        {
                            done = true;
                            eat = false;
                        }
                        else if (curContext != ParseContext.Parenthesis)
                        {
                            done = true;
                        }
                        break;

                    case TokenKind.INTEGER:
                    case TokenKind.DECIMAL:
                    case TokenKind.IMAG_INT:
                    case TokenKind.IMAG_DEC:
                        SetupExpr(ParseNumber());
                        break;
                    
                    case TokenKind.TEXT:
                        SetupExpr(new Text(curToken.Value));
                        break;
                    
                    case TokenKind.IDENTIFIER:
                        var identToken = curToken;
                        eat = false;
                        Eat();
                        if (Peek(TokenKind.SQUARE_START))
                            SetupExpr(ParseFunction(identToken.Value), identToken);
                        else
                            SetupExpr(new Variable(identToken.Value, curScope));
                        break;

                    case TokenKind.TRUE:
                        SetupExpr(new Boolean(true));
                        break;
                    case TokenKind.FALSE:
                        SetupExpr(new Boolean(false));
                        break;
                    case TokenKind.NULL:
                        SetupExpr(new Null());
                        break;
                    case TokenKind.SELF:
                        SetupExpr(new Self());
                        break;

                    case TokenKind.PARENT_START:
                        eat = false;
                        SetupExpr(ParseParenthesis());
                        break;
                    case TokenKind.SQUARE_START:
                        eat = false;
                        SetupExpr(ParseList());
                        break;
                    case TokenKind.CURLY_START:
                        eat = false;
                        SetupExpr(ParseScope());
                        break;

                    case TokenKind.HASH:
                        eat = false;
                        ParseComment();
                        break;

                    case TokenKind.PARENT_END:
                        if (curContext == ParseContext.Parenthesis)
                        {
                            done = true;
                            eat = false;
                        }
                        else
                            ReportError("Unexpected ')' in " + curContext);
                        break;
                    case TokenKind.SQUARE_END:
                        if (curContext == ParseContext.List)
                        {
                            done = true;
                            eat = false;
                        }
                        else
                            ReportError("Unexpected ']' in " + curContext);
                        break;
                    case TokenKind.CURLY_END:
                        if (curContext == ParseContext.ScopeMulti)
                        {
                            done = true;
                            eat = false;
                        }
                        else
                            ReportError("Unexpected '}' in " + curContext);
                        break;
                    
                    case TokenKind.TILDE:
                        if (expectUnary)
                            SetupUnOp(new Referation());
                        else
                            ReportError(curToken + " is not supported as binary operator");
                        break;

                    case TokenKind.ADD:
                        if (!expectUnary) // Ignore Unary +
                            SetupBiOp(new Add());
                        break;
                    case TokenKind.SUB:
                        if (expectUnary)
                            SetupUnOp(new Minus());
                        else
                            SetupBiOp(new Sub());
                        break;
                    case TokenKind.NEG:
                        if (expectUnary)
                            SetupUnOp(new Negation());
                        else
                            ReportError(curToken + " is not supported as binary operator");
                        break;
                    case TokenKind.MUL:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Mul());
                        break;
                    case TokenKind.DIV:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Div());
                        break;
                    case TokenKind.MOD:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Mod());
                        break;
                    case TokenKind.EXP:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Exp());
                        break;

                    case TokenKind.ASSIGN:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                        {
                            eat = false;
                            Eat();
                            SetupBiOp(new Assign());
                            while (Eat(TokenKind.NEW_LINE)); // Allow assignment on new line
                        }
                        break;
                    case TokenKind.EQUAL:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Equal());
                        break;
                    case TokenKind.BOOL_EQUAL:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new BooleanEqual());
                        break;
                    case TokenKind.NOT_EQUAL:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new NotEqual());
                        break;
                    case TokenKind.LESS_EQUAL:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new LesserEqual());
                        break;
                    case TokenKind.GREAT_EQUAL:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new GreaterEqual());
                        break;
                    case TokenKind.LESS:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Lesser());
                        break;
                    case TokenKind.GREAT:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Greater());
                        break;
                    case TokenKind.AND:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new And());
                        break;
                    case TokenKind.OR:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Or());
                        break;

                    case TokenKind.DOT:
                        if (expectUnary)
                            ReportError(curToken + " is not supported as unary operator");
                        else
                            SetupBiOp(new Dot());
                        break;

                    default:
                        ReportError("Unexpected '" + curToken.ToString() + "'");
                        break;
                }

                if (Error)
                {
                    unaryStack.Pop();
                    exprStack.Pop();
                    binaryStack.Pop();
                    return new Null();
                }

                if (eat)
                    Eat();
            }
                
            unaryStack.Pop();
            return CreateAst(exprStack.Pop(), binaryStack.Pop());
        }

        public void SetupExpr(Expression expr, Token tok = null)
        {
            expr.Position = tok != null ? tok.Position : curToken.Position;

            expr.CurScope = curScope;

            while (curUnaryStack.Count > 0)
            {
                var unop = curUnaryStack.Dequeue();
                unop.Child = expr;
                expr = unop;
            }

            curExprStack.Enqueue(expr);
            expectUnary = false;

            if (curExprStack.Count != curBinaryStack.Count + 1)
                ReportError("Missing operator");
        }

        public void SetupUnOp(UnaryOperator op)
        {
            op.Position = curToken.Position;
            op.CurScope = curScope;

            curUnaryStack.Enqueue(op);
        }

        public void SetupBiOp(BinaryOperator op)
        {
            op.Position = curToken.Position;
            op.CurScope = curScope;

            curBinaryStack.Enqueue(op);
            expectUnary = true;

            if (curExprStack.Count != curBinaryStack.Count)
                ReportError("Missing operand");
        }

        public Expression CreateAst(Queue<Expression> exprs, Queue<BinaryOperator> biops)
        {
            Expression left, right;
            BinaryOperator curOp, nextOp, top;

            if (exprs.Count == 0)
                return new Null();

            if (exprs.Count == 1 && biops.Count == 0)
                return exprs.Dequeue();

            if (exprs.Count != 1 + biops.Count)
            {
                ReportError("Missing operand");
                return new Null();
            }
               

            top = biops.Peek();
            left = exprs.Dequeue();

            while (biops.Count > 0)
            {
                curOp = biops.Dequeue();
                curOp.Left = left;

                if (biops.Count > 0)
                {
                    nextOp = biops.Peek();

                    if (curOp.Priority >= nextOp.Priority)
                    {
                        right = exprs.Dequeue();
                        curOp.Right = right;

                        if (top.Priority >= nextOp.Priority)
                        {
                            left = top;
                            top = nextOp;
                        }
                        else
                        {
                            left = curOp;
                            top.Right = nextOp;
                        }
                    }
                    else
                    {
                        left = exprs.Dequeue();

                        right = nextOp;
                        curOp.Right = right;
                    }
                }
                else
                {
                    right = exprs.Dequeue();
                    curOp.Right = right;
                }
            }

            return top;
        }

        public Expression ParseNumber()
        {
            Int64 intRes;
            decimal decRes;

            switch (curToken.Kind)
            {
                case TokenKind.INTEGER:
                    if (Int64.TryParse(curToken.Value, out intRes))
                        return new Integer(intRes);
                    else
                    {
                        ReportError("Int overflow");
                        return new Null();
                    }
                
                case TokenKind.DECIMAL:
                    if (decimal.TryParse(curToken.Value, out decRes))
                        return new Irrational(decRes);
                    else
                    {
                        ReportError("Decimal overflow");
                        return new Null();
                    }
                case TokenKind.IMAG_INT:
                    if (Int64.TryParse(curToken.Value, out intRes))
                        return new Complex(new Integer(0), new Integer(intRes));
                    else
                    {
                        ReportError("Imaginary int overflow");
                        return new Null();
                    }
                
                case TokenKind.IMAG_DEC:
                    if (decimal.TryParse(curToken.Value, out decRes))
                        return new Complex(new Integer(0), new Irrational(decRes));
                    else
                    {
                        ReportError("Imaginary decimal overflow");
                        return new Null();
                    }
                
                default:
                    throw new Exception("Wrong number token");
            }
        }
            
        public Expression ParseFunction(string sym)
        {
            List res = ParseList();
            var args = res.items;
                
            switch (sym.ToLower())
            {
                case "abs":
                    return new AbsFunc(args, curScope);
                case "sin":
                    return new SinFunc(args, curScope);
                case "cos":
                    return new CosFunc(args, curScope);
                case "tan":
                    return new TanFunc(args, curScope);
                case "asin":
                    return new AsinFunc(args, curScope);
                case "acos":
                    return new AcosFunc(args, curScope);
                case "atan":
                    return new AtanFunc(args, curScope);
                case "sqrt":
                    return new SqrtFunc(args, curScope);
                case "reduce":
                    return new ReduceFunc(args, curScope);
                case "expand":
                    return new ExpandFunc(args, curScope);
                case "range":
                    return new RangeFunc(args, curScope);
                case "solve":
                    return new SolveFunc(args, curScope);
                case "type":
                    return new TypeFunc(args, curScope);
                case "eval":
                    return new EvalFunc(args, curScope);
                case "print":
                    return new PrintFunc(args, curScope);
                case "plot":
                    return new PlotFunc(args, curScope);
                case "paraplot":
                    return new ParaPlotFunc(args, curScope);
                case "line":
                    return new LineFunc(args, curScope);
                default:
                    return new VariableFunc(sym.ToLower(), args, curScope);
            }
        }

        public ErrorData ReportError(ErrorData error)
        {
            curScope.Errors.Add(error);

            return error;
        }

        public ErrorData ReportError(string msg)
        {
            var error = new ErrorData(msg);
            error.Position = curToken.Position;

            curScope.Errors.Add(error);

            return error;
        }
    }
}
