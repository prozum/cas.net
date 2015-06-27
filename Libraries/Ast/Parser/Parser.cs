using System;
using System.Collections.Generic;

namespace Ast
{

    public enum ParseContext
    {
        List,
        ScopeSingle,
        ScopeMulti,
        ScopeGlobal,
        If,
        For,
        While,
        Colon,
        Ret,
        Import,
        Parenthesis
    }

    public class Parser
    {
        Stack<Scope> ScopeStack = new Stack<Scope>();
        Stack<ParseContext> ContextStack = new Stack<ParseContext>();

        Stack<Queue<Expression>> ExprStack = new Stack<Queue<Expression>>();
        Stack<Queue<PrefixOperator>> PrefixStack = new Stack<Queue<PrefixOperator>>();
        Stack<Queue<PostfixOperator>> PostfixStack = new Stack<Queue<PostfixOperator>>();
        Stack<Queue<BinaryOperator>> BinaryStack = new Stack<Queue<BinaryOperator>>();

        Evaluator Evaluator;
        Scope CurScope { get { return ScopeStack.Peek(); } }
        ParseContext CurContext { get { return ContextStack.Peek(); } }

        Queue<Expression> CurExprStack { get { return ExprStack.Peek(); } }
        Queue<PrefixOperator> CurPrefixStack { get { return PrefixStack.Peek(); } }
        Queue<PostfixOperator> CurPostfixStack { get { return PostfixStack.Peek(); } }
        Queue<BinaryOperator> CurBinaryStack { get { return BinaryStack.Peek(); } }

        readonly Token EOS = new Token(TokenKind.END_OF_STRING, "END_OF_STRING", new Pos());

        Queue<Token> Tokens;
        Token CurToken { get { return Tokens.Count > 0 ? Tokens.Peek() : EOS; } }

        bool Error { get { return Evaluator.Error != null; } }
        bool ExpectPrefix = true;

        public Parser(Evaluator eval)
        {
            Evaluator = eval;
        }

        public void Parse(string parseString)
        {
            Evaluator.Error = null;

            Tokens = Scanner.Tokenize(parseString, out Evaluator.Error);
            if (Error)
                return;
                
            ParseScope(ScopeContext.Default, true);

            if (Error)
                Clear();
        }

        #region Peek Eat

        public bool Peek()
        {
            return Tokens.Count > 0;
        }

        public bool Peek(TokenKind kind)
        {
            return CurToken.Kind == kind;
        }

        public bool Eat()
        {
            if (Tokens.Count > 0)
                Tokens.Dequeue();

            return Tokens.Count > 0;
        }

        public bool Eat(TokenKind kind)
        {
            if (Tokens.Count > 0 && Tokens.Peek().Kind == kind)
            {
                Tokens.Dequeue();
                return true;
            }

            return false;
        }

        #endregion

        public void Clear()
        {
            ScopeStack.Clear();
            ContextStack.Clear();
            ExprStack.Clear();
            PrefixStack.Clear();
            PostfixStack.Clear();
            BinaryStack.Clear();
        }

        public Expression ParseScope(ScopeContext cx = ScopeContext.Default, bool global = false)
        {
            while (Eat(TokenKind.NEW_LINE));

            if (global)
            {
                ContextStack.Push(ParseContext.ScopeGlobal);
                ScopeStack.Push(Evaluator);
            }
            else if (Eat(TokenKind.CURLY_START))
            {
                ContextStack.Push(ParseContext.ScopeMulti);
                ScopeStack.Push(new Scope(CurScope, cx));
            }
            else
            {
                ContextStack.Push(ParseContext.ScopeSingle);
                ScopeStack.Push(new Scope(CurScope, cx));
            }

            ParseExpressions();

            if (CurContext == ParseContext.ScopeMulti && !Eat(TokenKind.CURLY_END))
                ReportError("Missing } bracket");
                
            ContextStack.Pop();
            return ScopeStack.Pop();
        }

        public void ParseExpressions()
        {
            while (true)
            {
                while (Eat(TokenKind.NEW_LINE));
                CurScope.Expressions.Add(ParseExpr());
                while (Eat(TokenKind.NEW_LINE));

                switch (CurToken.Kind)
                {
                    case TokenKind.CURLY_END:
                    case TokenKind.ELIF:
                    case TokenKind.ELSE:
                    case TokenKind.END_OF_STRING:
                        return;

                    case TokenKind.SEMICOLON:
                        Eat();
                        break;
                }

                if (CurContext == ParseContext.ScopeSingle)
                    return;

                if (Error)
                    return;
            }
        }

        public Expression ParseExpr(bool eat = false)
        {
            bool done = false;

            if (eat)
                Eat();

            ExprStack.Push(new Queue<Expression>());
            PrefixStack.Push(new Queue<PrefixOperator>());
            PostfixStack.Push(new Queue<PostfixOperator>());
            BinaryStack.Push(new Queue<BinaryOperator>());

            ExpectPrefix = true;

            while (!done)
            {
                switch (CurToken.Kind)
                {
                    case TokenKind.END_OF_STRING:
                        done = true;
                        break;
                    case TokenKind.NEW_LINE:
                        done |= CurContext != ParseContext.Parenthesis;
                        break;

                    case TokenKind.SEMICOLON:
                        if (CurContext == ParseContext.List)
                            ReportError("Unexpected ';' in " + CurContext);
                        else
                            done = true;
                        break;
                    case TokenKind.COMMA:
                        if (CurContext == ParseContext.List)
                            done = true;
                        else
                            ReportError("Unexpected ',' in " + CurContext);
                        Eat();
                        break;


                    case TokenKind.IF:
                        done = true;
                        SetupExpr(ParseIf(), false);
                        break;
                    case TokenKind.FOR:
                        SetupExpr(ParseFor(), false);
                        break;
                    case TokenKind.WHILE:
                        SetupExpr(ParseWhile(), false);
                        break;
                    case TokenKind.RET:
                        done = true;
                        ContextStack.Push(ParseContext.Ret);
                        SetupExpr(new RetExpr(ParseExpr(true), CurScope), false);
                        ContextStack.Pop();
                        break;
                    case TokenKind.IMPORT:
                        done = true;
                        ContextStack.Push(ParseContext.Import);
                        SetupExpr(new ImportExpr(ParseExpr(true), CurScope), false);
                        ContextStack.Pop();
                        break;
                    case TokenKind.COLON:
                        if (CurContext == ParseContext.Colon)
                            done = true;
                        else
                            ReportError("Unexpected ':' in " + CurContext);
                        break;
                    
                    case TokenKind.ELIF:
                        if (CurScope.Context == ScopeContext.If)
                            done = true;
                        else
                            ReportError("Unexpected 'elif' in " + CurContext);
                        break;
                    case TokenKind.ELSE:
                        if (CurScope.Context == ScopeContext.If)
                            done = true;
                        else
                            ReportError("Unexpected 'else' in " + CurContext);
                        break;


                    case TokenKind.INTEGER:
                    case TokenKind.DECIMAL:
                    case TokenKind.IMAG_INT:
                    case TokenKind.IMAG_DEC:
                        SetupExpr(ParseNumber(), true);
                        break;
                    
                    case TokenKind.TEXT:
                        SetupExpr(new Text(CurToken.Value),true);
                        break;
                    
                    case TokenKind.IDENTIFIER:
                        SetupExpr(new Variable(CurToken.Value, CurScope), true);
                        break;

                    case TokenKind.TRUE:
                        SetupExpr(new Boolean(true),true);
                        break;
                    case TokenKind.FALSE:
                        SetupExpr(new Boolean(false),true);
                        break;
                    case TokenKind.NULL:
                        SetupExpr(new Null(),true);
                        break;
                    case TokenKind.SELF:
                        SetupExpr(new Self(),true);
                        break;

                    case TokenKind.PARENT_START:
                        SetupExpr(ParseParenthesis(),false);
                        break;
                    case TokenKind.SQUARE_START:
                        SetupExpr(ParseList(),false);
                        break;
                    case TokenKind.CURLY_START:
                        SetupExpr(ParseScope(),false);
                        break;

                    case TokenKind.HASH:
                        ParseComment();
                        break;

                    case TokenKind.PARENT_END:
                        if (CurContext == ParseContext.Parenthesis)
                            done = true;
                        else
                            ReportError("Unexpected ')' in " + CurContext);
                        break;
                    case TokenKind.SQUARE_END:
                        if (CurContext == ParseContext.List)
                            done = true;
                        else
                            ReportError("Unexpected ']' in " + CurContext);
                        break;
                    case TokenKind.CURLY_END:
                        if (CurContext == ParseContext.ScopeMulti || CurContext == ParseContext.ScopeSingle ||
                            CurContext == ParseContext.Ret || CurContext == ParseContext.Import)
                            done = true;
                        else
                            ReportError("Unexpected '}' in " + CurContext);
                        break;
                    
                    case TokenKind.TILDE:
                        if (ExpectPrefix)
                            SetupPrefixOp(new Referation());
                        else
                            ReportError(CurToken + " is not supported as binary operator");
                        break;

                    case TokenKind.ADD:
                        if (!ExpectPrefix) // Ignore Unary +
                            SetupBinaryOp(new Add());
                        break;
                    case TokenKind.SUB:
                        if (ExpectPrefix)
                            SetupPrefixOp(new Minus());
                        else
                            SetupBinaryOp(new Sub());
                        break;
                    case TokenKind.NEG:
                        if (ExpectPrefix)
                            SetupPrefixOp(new Negation());
                        else
                            ReportError(CurToken + " is not supported as binary operator");
                        break;
                    case TokenKind.MUL:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Mul());
                        break;
                    case TokenKind.DIV:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Div());
                        break;
                    case TokenKind.MOD:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Mod());
                        break;
                    case TokenKind.EXP:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Exp());
                        break;

                    case TokenKind.ASSIGN:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                        {
                            SetupBinaryOp(new Assign());
                            while (Eat(TokenKind.NEW_LINE)); // Allow assignment on new line
                        }
                        break;
                    case TokenKind.EQUAL:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Equal());
                        break;
                    case TokenKind.BOOL_EQUAL:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new BooleanEqual());
                        break;
                    case TokenKind.NOT_EQUAL:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new NotEqual());
                        break;
                    case TokenKind.LESS_EQUAL:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new LesserEqual());
                        break;
                    case TokenKind.GREAT_EQUAL:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new GreaterEqual());
                        break;
                    case TokenKind.LESS:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Lesser());
                        break;
                    case TokenKind.GREAT:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Greater());
                        break;
                    case TokenKind.AND:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new And());
                        break;
                    case TokenKind.OR:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Or());
                        break;

                    case TokenKind.DOT:
                        if (ExpectPrefix)
                            ReportError(CurToken + " is not supported as unary operator");
                        else
                            SetupBinaryOp(new Dot());
                        break;

                    default:
                        ReportError("Unexpected '" + CurToken.ToString() + "'");
                        break;
                }

                if (Error)
                {
                    PrefixStack.Pop();
                    PostfixStack.Pop();
                    ExprStack.Pop();
                    BinaryStack.Pop();
                    return null;
                }
            }
                
            PrefixStack.Pop();
            PostfixStack.Pop();
            return CreateAst(ExprStack.Pop(), BinaryStack.Pop());
        }

        public void ParsePostfix()
        {
            while (Peek(TokenKind.SQUARE_START))
            {
                var op = new Call(ParseList(), CurScope);
                if (Error)
                    return;

                op.Position = CurToken.Position;
                op.CurScope = CurScope;
                CurPostfixStack.Enqueue(op);
            }
        }

        public void SetupExpr(Expression expr, bool eat)
        {
            if (Error)
                return;

            if (eat)
                Eat();

            expr.Position = CurToken.Position;
            expr.CurScope = CurScope;

            ParsePostfix();

            if (Error)
                return;

            while (CurPostfixStack.Count > 0)
            {
                var postfix = CurPostfixStack.Dequeue();
                postfix.Child = expr;
                expr = postfix;
            }

            while (CurPrefixStack.Count > 0)
            {
                var prefix = CurPrefixStack.Dequeue();
                prefix.Child = expr;
                expr = prefix;
            }

            CurExprStack.Enqueue(expr);
            ExpectPrefix = false;

            if (CurExprStack.Count != CurBinaryStack.Count + 1)
                ReportError("Missing operator");
        }

        public void SetupPrefixOp(PrefixOperator op)
        {
            Eat();

            op.Position = CurToken.Position;
            op.CurScope = CurScope;

            CurPrefixStack.Enqueue(op);
        }

        public void SetupBinaryOp(BinaryOperator op)
        {
            Eat();

            op.Position = CurToken.Position;
            op.CurScope = CurScope;

            CurBinaryStack.Enqueue(op);
            ExpectPrefix = true;

            if (CurExprStack.Count != CurBinaryStack.Count)
                ReportError("Missing operand");
        }

        public Expression CreateAst(Queue<Expression> exprs, Queue<BinaryOperator> biops)
        {
            Expression left;
            BinaryOperator curOp, nextOp, top, cmpOp;

            if (exprs.Count == 0)
                return Constant.Null;

            if (exprs.Count == 1 && biops.Count == 0)
                return exprs.Dequeue();

            if (exprs.Count != 1 + biops.Count)
                return ReportError("Missing operand");
               

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
                        curOp.Right = exprs.Dequeue();

                        cmpOp = curOp;
                        while (cmpOp.Parent != null)
                        {
                            if (nextOp.Priority > (cmpOp.Parent as BinaryOperator).Priority)
                            {
                                break;
                            }
                            else
                            {
                                cmpOp = cmpOp.Parent as BinaryOperator;
                            }
                        }

                        if (cmpOp.Parent == null)
                        {
                            top = nextOp;
                            left = cmpOp;
                        }
                        else
                        {
                            left = (cmpOp.Parent as BinaryOperator).Right;
                            (cmpOp.Parent as BinaryOperator).Right = nextOp;
                        }
                    }
                    else
                    {
                        left = exprs.Dequeue();

                        curOp.Right = nextOp;
                    }
                }
                else
                {
                    curOp.Right = exprs.Dequeue();
                }
            }

            return top;
        }

        public Expression ParseNumber()
        {
            Int64 intRes;
            decimal decRes;

            switch (CurToken.Kind)
            {
                case TokenKind.INTEGER:
                    if (Int64.TryParse(CurToken.Value, out intRes))
                        return new Integer(intRes);
                    else
                        return ReportError("Int overflow");
                
                case TokenKind.DECIMAL:
                    if (decimal.TryParse(CurToken.Value, out decRes))
                        return new Irrational(decRes);
                    else
                        return ReportError("Decimal overflow");
                case TokenKind.IMAG_INT:
                    if (Int64.TryParse(CurToken.Value, out intRes))
                        return new Complex(new Integer(0), new Integer(intRes));
                    else
                        return ReportError("Imaginary int overflow");
                
                case TokenKind.IMAG_DEC:
                    if (decimal.TryParse(CurToken.Value, out decRes))
                        return new Complex(new Integer(0), new Irrational(decRes));
                    else
                        return ReportError("Imaginary decimal overflow");
                
                default:
                    throw new Exception("Wrong number token");
            }
        }

        public Expression ParseIf()
        {
            Expression cond;
            Expression expr;

            Eat();

            ContextStack.Push(ParseContext.If);
            var ifexpr = new IfExpr(CurScope);

            cond = ParseColon();
            if (Error)
                return null;
            ifexpr.Conditions.Add(cond);

            expr = ParseScope(ScopeContext.If);
            if (Error)
                return null;
            ifexpr.Expressions.Add(expr);

            while (Eat(TokenKind.NEW_LINE));
            while (Eat(TokenKind.ELIF))
            {
                cond = ParseColon();
                if (Error)
                    return null;
                ifexpr.Conditions.Add(cond);

                expr = ParseScope(ScopeContext.If);
                if (Error)
                    return null;
                ifexpr.Expressions.Add(expr);

                while (Eat(TokenKind.NEW_LINE));
            }

            while (Eat(TokenKind.NEW_LINE));
            if (Eat(TokenKind.ELSE))
            {
                Eat(TokenKind.COLON); // Optional colon

                expr = ParseScope(ScopeContext.If);
                if (Error)
                    return null;
                ifexpr.Expressions.Add(expr);
            }

            ContextStack.Pop();
            return ifexpr;
        }

        public Expression ParseFor()
        {
            Eat();

            ContextStack.Push(ParseContext.For);
            var forexpr = new ForExpr(CurScope);

            if (Peek(TokenKind.IDENTIFIER))
            {
                forexpr.Var = CurToken.Value;
                Eat();
            }
            else
                return ReportError("For: Missing symbol");

            if (!Eat(TokenKind.IN))
                return ReportError("For: Missing in");

            forexpr.List = ParseColon();
            if (Error)
                return null;

            forexpr.ForScope = ParseScope();
            if (Error)
                return null;

            ContextStack.Pop();
            return forexpr;
        }

        public Expression ParseWhile()
        {
            Expression expr;
            Eat();

            ContextStack.Push(ParseContext.While);
            var whileexpr = new WhileExpr(CurScope);

            whileexpr.Condition = ParseColon();
            if (Error)
                return null;

            expr = ParseScope();
            if (Error)
                return null;
            whileexpr.Condition.CurScope = whileexpr.WhileScope = (Scope)expr;

            ContextStack.Pop();
            return whileexpr;
        }

        public Expression ParseColon()
        {
            ContextStack.Push(ParseContext.Colon);
            var res = ParseExpr();
            ContextStack.Pop();

            if (!Eat(TokenKind.COLON))
                return ReportError("Missing :");

            return res;
        }

        public List ParseList()
        {
            var list = new List();

            Eat();
            ContextStack.Push(ParseContext.List);

            while (Tokens.Count > 0)
            {
                if (!(Eat(TokenKind.COMMA) || Eat(TokenKind.NEW_LINE)))
                    list.Items.Add(ParseExpr());

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

            ContextStack.Pop();

            if (list.Items.Count == 1 && list.Items[0] is Null)
                list.Items.Clear();

            return list;
        }

        public Expression ParseParenthesis()
        {
            Eat();
            ContextStack.Push(ParseContext.Parenthesis);

            Expression parent = ParseExpr();

            if (Error)
                return null;

            if (!Eat(TokenKind.PARENT_END))
                ReportError("Missing ) bracket");

            ContextStack.Pop();

            return parent;
        }

        public void ParseComment()
        {
            do
            {
                Eat();
            }
            while (CurToken.Kind != TokenKind.NEW_LINE && CurToken.Kind != TokenKind.END_OF_STRING);
        }

        public Expression ReportError(string msg)
        {
            var error = new Error(msg);
            error.Position = CurToken.Position;

            Evaluator.Error = error;

            return null;
        }
    }
}

