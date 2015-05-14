using System;
using System.Collections.Generic;

namespace Ast
{

    public enum ParseContext
    {
        List,
        Scope,
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

        Queue<Token> tokens;
        Token tok;
        bool expectUnary = true;

        public void Parse(string parseString)
        {
            Parse(parseString, new Scope());
        }

        public void Parse(string parseString, Scope global)
        {
            global.Error = null;
            scopeStack.Push(global);
            contextStack.Push(ParseContext.Scope);

            tokens = Scanner.Tokenize(parseString, out global.Error);
            if (global.Error != null)
                return;

            tok = tokens.Peek();
            ParseScope(TokenKind.END_OF_STRING, false);
            contextStack.Pop();
        }

        public Scope ParseScope(TokenKind stopToken, bool newScope = true)
        {
            Scope res;

            if (newScope)
                scopeStack.Push(new Scope(curScope));

            res = curScope;

            contextStack.Push(ParseContext.Scope);

            while (tokens.Count > 0)
            {
                if (Eat(stopToken))
                {
                    if (newScope)
                        scopeStack.Pop();

                    contextStack.Pop();

                    return res;
                }

                Peek();
                switch (tok.Kind)
                {
                    case TokenKind.IF:
                        Eat();
                        res.Statements.Add(ParseIfStmt());
                        break;
                    case TokenKind.FOR:
                        Eat();
                        res.Statements.Add(ParseForStmt());
                        break;
                    case TokenKind.RET:
                        Eat();
                        res.Statements.Add(new RetStmt(ParseExpr(stopToken), curScope));
                        break;
                    
                    case TokenKind.PRINT:
                        Eat();
                        if (Eat(TokenKind.SQUARE_START))
                            res.Statements.Add(new PrintFunc(ParseList().items, curScope));
                        else
                            ReportSyntaxError("Print syntax: print[expr]");
                        break;
                    case TokenKind.PLOT:
                        Eat();
                        if (Eat(TokenKind.SQUARE_START))
                            res.Statements.Add(new PlotFunc(ParseList().items, curScope));
                        else
                            ReportSyntaxError("Print syntax: plot[expr,symbol]");
                            break;

                    case TokenKind.SEMICOLON:
                    case TokenKind.NEW_LINE:
                        Eat();
                        break;

                    default:
                        res.Statements.Add(new ExprStmt(ParseExpr(stopToken), curScope));
                        break;
                }

                if (res.Error != null)
                {
                    res.Statements.Clear();
                    return res;
                }
            }

            ReportSyntaxError("Missing " + stopToken.ToString());

            return res;
        }

        #region Peek Eat

        public bool Peek()
        {
            if (tokens.Count > 0)
            {
                tok = tokens.Peek();
                return true;
            }
                
            return true;
        }

        public bool Peek(TokenKind kind)
        {
            if (tokens.Count > 0 && tokens.Peek().Kind == kind)
            {
                tok = tokens.Peek();
                return true;
            }
                
            return false;
        }

        public bool Eat()
        {
            if (tokens.Count > 0)
                tok = tokens.Dequeue();

            return tokens.Count > 0;
        }

        public bool Eat(TokenKind kind)
        {
            if (tokens.Count > 0 && tokens.Peek().Kind == kind)
            {
                tok = tokens.Dequeue();
                return true;
            }
                
            return false;
        }

        #endregion

        public IfStmt ParseIfStmt()
        {
            var stmt = new IfStmt(curScope);

            stmt.conditions.Add(ParseExpr(TokenKind.COLON));

            if (!Eat(TokenKind.COLON))
            {
                ReportSyntaxError("If syntax: if bool: {}");
                return null;
            }

            if (Eat(TokenKind.CURLY_START))
                stmt.expressions.Add(ParseScope(TokenKind.CURLY_END));
            else
            {
                stmt.expressions.Add(ParseScope(TokenKind.SEMICOLON));

                if (!Eat(TokenKind.SEMICOLON))
                {
                    ReportSyntaxError("If syntax: if bool: {}");
                    return null;
                }
            }

            while (Eat(TokenKind.ELIF))
            {
                stmt.conditions.Add(ParseExpr(TokenKind.COLON));

                if (Eat(TokenKind.COLON))
                    stmt.conditions.Add(ParseExpr(TokenKind.COLON));
                else
                {
                    ReportSyntaxError("If syntax: if bool: {}");
                    return null;
                }

                if (Eat(TokenKind.CURLY_START))
                    stmt.expressions.Add(ParseScope(TokenKind.CURLY_END));
                else
                    stmt.expressions.Add(ParseScope(TokenKind.SEMICOLON));
                //tokens.Dequeue(); // Skip '}' or ';'
            }

            if (Eat(TokenKind.ELSE))
            {

                if (Eat(TokenKind.CURLY_START))
                    stmt.expressions.Add(ParseScope(TokenKind.CURLY_END));
                else
                    stmt.expressions.Add(ParseScope(TokenKind.SEMICOLON));
                //tokens.Dequeue(); // Skip '}' or ';'
            }

            return stmt;
        }

        public ForStmt ParseForStmt()
        {
            var stmt = new ForStmt(curScope);

            if (Peek(TokenKind.IDENTIFIER))
            {
                stmt.sym = tok.Value;
                Eat();
            }
            else
            {
                ReportSyntaxError("If syntax: for symbol in list:");
                return null;
            }

            if (!Eat(TokenKind.IN))
            {
                ReportSyntaxError("If syntax: for symbol in list:");
                return null;
            }

            var list = ParseExpr(TokenKind.COLON);

            if (!Eat(TokenKind.COLON))
            {
                ReportSyntaxError("If syntax: for symbol in list:", true);
                return null;
            }

            if (list is Error)
            {
                //TODO FIX THIS
                //return stmt.list;
                return null;
            }

            if (list is Variable)
                list = curScope.GetVar((list as Variable).identifier);

            if (list != null || list is List)
                stmt.list = list as List;
            else
            {
                ReportSyntaxError("For: list argument is not a list");
                return null;
            }
                   

            if (Eat(TokenKind.CURLY_START))
                stmt.expr = ParseScope(TokenKind.CURLY_END);
            else
                stmt.expr = ParseScope(TokenKind.SEMICOLON);

            return stmt;
        }

        public List ParseList()
        {
            var list = new List();

            contextStack.Push(ParseContext.List);

            while (tokens.Count > 0)
            {
                if (Eat(TokenKind.SQUARE_END))
                {
                    contextStack.Pop();
                    return list;
                }

                if (!(Eat(TokenKind.COMMA) || Eat(TokenKind.NEW_LINE)))
                    list.items.Add(ParseExpr(TokenKind.SQUARE_END));
            }

            ReportSyntaxError("Missing ] bracket");

            return list;
        }

        public Expression ParseExpr(TokenKind stopToken)
        {
            bool done = false;

            exprStack.Push(new Queue<Expression>());
            unaryStack.Push(new Queue<UnaryOperator>());
            binaryStack.Push(new Queue<BinaryOperator>());

            expectUnary = true;

            while (!done)
            {
                if (Peek(stopToken))
                    break;

                Eat(); // Get switch token

                switch (tok.Kind)
                {
                    case TokenKind.END_OF_STRING:
                        done = true;
                        break;

                    case TokenKind.SEMICOLON:
                        if (curContext == ParseContext.Scope)
                            done = true;
                        else
                            ReportSyntaxError("Unexpected ';' in " + curContext);
                        break;
                    
                    case TokenKind.COMMA:
                        if (curContext == ParseContext.List)
                            done = true;
                        else
                            ReportSyntaxError("Unexpected ',' in " + curContext);
                        break;

                    case TokenKind.NEW_LINE:
                        done |= curContext != ParseContext.Parenthesis;
                        break;

                    case TokenKind.INTEGER:
                    case TokenKind.DECIMAL:
                    case TokenKind.IMAG_INT:
                    case TokenKind.IMAG_DEC:
                        SetupExpr(ParseNumber());
                        break;
                    
                    case TokenKind.TEXT:
                        SetupExpr(new Text(tok.Value));
                        break;
                    
                    case TokenKind.IDENTIFIER:
                        var curTok = tok;
                        if (Peek(TokenKind.SQUARE_START))
                            SetupExpr(ParseFunction(curTok.Value), curTok);
                        else
                            SetupExpr(new Symbol(curTok.Value, curScope));
                        break;

                    case TokenKind.TRUE:
                        SetupExpr(new Boolean(true));
                        break;
                    case TokenKind.FALSE:
                        SetupExpr(new Boolean(false));
                        break;

                    case TokenKind.PARENT_START:
                        contextStack.Push(ParseContext.Parenthesis);
                        SetupExpr(ParseExpr(TokenKind.PARENT_END));
                        contextStack.Pop();
 
                        if (!Eat(TokenKind.PARENT_END))
                            ReportSyntaxError("Missing )");
                        break;
                    case TokenKind.SQUARE_START:
                        SetupExpr(ParseList());
                        break;
                    case TokenKind.CURLY_START:
                        SetupExpr(ParseScope(TokenKind.CURLY_END, true));
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
                            ReportSyntaxError("Unexpected: '!'");
                        break;

                    case TokenKind.MUL:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Mul());
                        break;
                    case TokenKind.DIV:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Div());
                        break;
                    case TokenKind.EXP:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Exp());
                        break;

                    case TokenKind.ASSIGN:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Assign());
                        break;
                    case TokenKind.EQUAL:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Equal());
                        break;
                    case TokenKind.BOOL_EQUAL:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new BooleanEqual());
                        break;
                    case TokenKind.NOT_EQUAL:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new NotEqual());
                        break;
                    case TokenKind.LESS_EQUAL:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new LesserEqual());
                        break;
                    case TokenKind.GREAT_EQUAL:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new GreaterEqual());
                        break;
                    case TokenKind.LESS:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Lesser());
                        break;
                    case TokenKind.GREAT:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Greater());
                        break;

                    case TokenKind.DOT:
                        if (expectUnary)
                            ReportSyntaxError(tok + " is not supported as unary operator");
                        else
                            SetupBiOp(new Dot());
                        break;

                    default:
                        ReportSyntaxError("Unexpected '" + tok.ToString() + "'");
                        break;
                }

                if (curScope.Error != null)
                    return curScope.Error;
            }
                
            unaryStack.Pop();
            return CreateAst(exprStack.Pop(), binaryStack.Pop());
        }

        public void SetupExpr(Expression expr, Token curTok = null)
        {
            if (curTok == null)
                expr.Position = tok.Position;
            else
                expr.Position = curTok.Position; 
            expr.Scope = curScope;

            while (curUnaryStack.Count > 0)
            {
                var unop = curUnaryStack.Dequeue();
                unop.Child = expr;
                expr = unop;
            }

            curExprStack.Enqueue(expr);
            expectUnary = false;

            if (curExprStack.Count != curBinaryStack.Count + 1)
                ReportSyntaxError("Missing operator");
        }

        public void SetupUnOp(UnaryOperator op)
        {
            op.Position = tok.Position;
            op.Scope = curScope;

            curUnaryStack.Enqueue(op);
        }

        public void SetupBiOp(BinaryOperator op)
        {
            op.Position = tok.Position;
            op.Scope = curScope;

            curBinaryStack.Enqueue(op);
            expectUnary = true;

            if (curExprStack.Count != curBinaryStack.Count)
                ReportSyntaxError("Missing operand");
        }

        public Expression CreateAst(Queue<Expression> exprs, Queue<BinaryOperator> biops)
        {
            Expression left, right;
            BinaryOperator curOp, nextOp, top;

            if (exprs.Count == 0)
            {
                return ReportSyntaxError("Missing expression");
                return null;
            }

            if (exprs.Count != 1 + biops.Count)
            {
                return ReportSyntaxError("Missing operand");
            }

            if (exprs.Count == 1 && biops.Count == 0)
                return exprs.Dequeue();
                

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

            switch (tok.Kind)
            {
                case TokenKind.INTEGER:
                    if (Int64.TryParse(tok.Value, out intRes))
                        return new Integer(intRes);
                    else
                    {
                        return ReportSyntaxError("Int overflow");
                    }
                
                case TokenKind.DECIMAL:
                    if (decimal.TryParse(tok.Value, out decRes))
                        return new Irrational(decRes);
                    else
                    {
                        return ReportSyntaxError("Decimal overflow");
                    }
                case TokenKind.IMAG_INT:
                    if (Int64.TryParse(tok.Value, out intRes))
                        return new Complex(new Integer(0), new Integer(intRes));
                    else
                    {
                        return ReportSyntaxError("Imaginary int overflow");
                    }
                
                case TokenKind.IMAG_DEC:
                    if (decimal.TryParse(tok.Value, out decRes))
                        return new Complex(new Integer(0), new Irrational(decRes));
                    else
                    {
                        return ReportSyntaxError("Imaginary decimal overflow");
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
                default:
                    return new UsrFunc(sym.ToLower(), args, curScope);
            }
        }

        public Error ReportSyntaxError(string msg, bool overwrite = false)
        {
            if (curScope.Error == null || overwrite)
            {
                curScope.Error = new Error("Parser: " + msg);
                curScope.Error.Position = tok.Position;
            }

            return curScope.Error;
        }
    }
}

