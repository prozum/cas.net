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
        Colon,
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

        Scope CurScope { get { return ScopeStack.Peek(); } }
        ParseContext CurContext { get { return ContextStack.Peek(); } }

        Queue<Expression> CurExprStack { get { return ExprStack.Peek(); } }
        Queue<PrefixOperator> CurPrefixStack { get { return PrefixStack.Peek(); } }
        Queue<PostfixOperator> CurPostfixStack { get { return PostfixStack.Peek(); } }
        Queue<BinaryOperator> CurBinaryStack { get { return BinaryStack.Peek(); } }

        readonly Token EOS = new Token(TokenKind.END_OF_STRING, "END_OF_STRING", new Pos());

        Queue<Token> Tokens;
        Token CurToken { get { return Tokens.Count > 0 ? Tokens.Peek() : EOS; } }
        List<Error> Errors = new List<Error>();
        bool Error { get { return Errors.Count > 0; } }
        bool ExpectPrefix = true;

        public Error Parse(string parseString)
        {
            return Parse(parseString, new Scope());
        }

        public Error Parse(string parseString, Scope global)
        {
            Errors.Clear();

            Tokens = Scanner.Tokenize(parseString, Errors);
            if (Errors.Count > 0)
                return Errors[0];

            ParseScope(false, global);

            if (Errors.Count > 0)
                return Errors[0];

            return null;
        }

        public void Clear()
        {
            ScopeStack.Clear();
            ContextStack.Clear();
            ExprStack.Clear();
            PrefixStack.Clear();
            PostfixStack.Clear();
            BinaryStack.Clear();
        }

        public Scope ParseScope(bool share = false, Scope global = null)
        {
            ParseContext cx;

            while (Eat(TokenKind.NEW_LINE));

            if (global != null)
            {
                cx = ParseContext.ScopeGlobal;
                ScopeStack.Push(global);
            }
            else if (Eat(TokenKind.CURLY_START))
            {
                cx = ParseContext.ScopeMulti;
                ScopeStack.Push(new Scope(CurScope, share));
            }
            else
            {
                cx = ParseContext.ScopeSingle;
                ScopeStack.Push(new Scope(CurScope, share));
            }

            ContextStack.Push(cx);
            ParseKeyExpressions();
            ContextStack.Pop();

            if (Error)
                return ScopeStack.Pop();

            if (cx == ParseContext.ScopeMulti && !Eat(TokenKind.CURLY_END))
                ReportError("Missing } bracket");
               
            return ScopeStack.Pop();
        }

        public void ParseKeyExpressions()
        {
            while (Tokens.Count > 0)
            {
                switch (CurToken.Kind)
                {
                    case TokenKind.IF:
                        Eat();
                        CurScope.Expressions.Add(ParseIf());
                        break;
                    case TokenKind.FOR:
                        Eat();
                        CurScope.Expressions.Add(ParseFor());
                        break;
                    case TokenKind.WHILE:
                        Eat();
                        CurScope.Expressions.Add(ParseWhile());
                        break;
                    case TokenKind.RET:
                        Eat();
                        CurScope.Expressions.Add(new RetExpr(ParseExpr(), CurScope));
                        break;
                    case TokenKind.IMPORT:
                        Eat();
                        CurScope.Expressions.Add(new ImportExpr(ParseExpr(), CurScope));
                        break;

                    case TokenKind.ELIF:
                    case TokenKind.ELSE:
                    case TokenKind.CURLY_END:
                    case TokenKind.END_OF_STRING:
                        return;

                    case TokenKind.SEMICOLON:
                    case TokenKind.NEW_LINE:
                        Eat();
                        if (CurContext == ParseContext.ScopeSingle)
                            return;
                        break;

                    default:
                        CurScope.Expressions.Add(ParseExpr());
                        break;
                }

                if (Error)
                {
                    CurScope.Expressions.Clear();
                    return;
                }
            }

            throw new Exception("ParseScope failed");
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

        public IfExpr ParseIf()
        {
            Expression cond;
            Expression expr;

            var stmt = new IfExpr(CurScope);

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

        public ForExpr ParseFor()
        {
            var stmt = new ForExpr(CurScope);

            if (Peek(TokenKind.IDENTIFIER))
            {
                stmt.Var = CurToken.Value;
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

        public WhileExpr ParseWhile()
        {
            Expression cond;
            Scope scope;

            var stmt = new WhileExpr(CurScope);

            cond = ParseColon();
            if (Error)
                return null;
            stmt.Condition = cond;

            scope = ParseScope();
            if (Error)
                return null;
            stmt.WhileScope = scope;
            stmt.Condition.CurScope = scope;

            return stmt;
        }

        public Expression ParseColon()
        {
            ContextStack.Push(ParseContext.Colon);
            var res = ParseExpr();
            ContextStack.Pop();

            if (!Eat(TokenKind.COLON))
                ReportError("Missing :");

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
                return parent;

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

        public Expression ParseExpr()
        {
            bool done = false;

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

                    case TokenKind.SEMICOLON:
                        if (CurContext == ParseContext.ScopeGlobal || CurContext == ParseContext.ScopeMulti)
                        {
                            done = true;
                            Eat();
                        }
                        else if (CurContext == ParseContext.ScopeSingle)
                            done = true;
                        else
                            ReportError("Unexpected ';' in " + CurContext);
                        break;

                    case TokenKind.COLON:
                        if (CurContext == ParseContext.Colon)
                            done = true;
                        else
                            ReportError("Unexpected ':' in " + CurContext);
                        break;
                    
                    case TokenKind.COMMA:
                        if (CurContext == ParseContext.List)
                            done = true;
                        else
                            ReportError("Unexpected ',' in " + CurContext);
                        Eat();
                        break;

                    case TokenKind.NEW_LINE:
                        if (CurContext != ParseContext.Parenthesis)
                            done = true;
                        Eat();
                        break;

                    case TokenKind.INTEGER:
                    case TokenKind.DECIMAL:
                    case TokenKind.IMAG_INT:
                    case TokenKind.IMAG_DEC:
                        SetupExpr(ParseNumber(),true);
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
                        if (CurContext == ParseContext.ScopeMulti || CurContext == ParseContext.ScopeSingle)
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
                    return Constant.Null;
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
            expr.Position = CurToken.Position;
            expr.CurScope = CurScope;

            if (eat)
                Eat();

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
            op.Position = CurToken.Position;
            op.CurScope = CurScope;
            Eat();

            CurPrefixStack.Enqueue(op);
        }

        public void SetupBinaryOp(BinaryOperator op)
        {
            op.Position = CurToken.Position;
            op.CurScope = CurScope;
            Eat();

            CurBinaryStack.Enqueue(op);
            ExpectPrefix = true;

            if (CurExprStack.Count != CurBinaryStack.Count)
                ReportError("Missing operand");
        }

        public Expression CreateAst(Queue<Expression> exprs, Queue<BinaryOperator> biops)
        {
            Expression left, right;
            BinaryOperator curOp, nextOp, top;

            if (exprs.Count == 0)
                return Constant.Null;

            if (exprs.Count == 1 && biops.Count == 0)
                return exprs.Dequeue();

            if (exprs.Count != 1 + biops.Count)
            {
                ReportError("Missing operand");
                return Constant.Null;
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

            switch (CurToken.Kind)
            {
                case TokenKind.INTEGER:
                    if (Int64.TryParse(CurToken.Value, out intRes))
                        return new Integer(intRes);
                    else
                    {
                        ReportError("Int overflow");
                        return Constant.Null;
                    }
                
                case TokenKind.DECIMAL:
                    if (decimal.TryParse(CurToken.Value, out decRes))
                        return new Irrational(decRes);
                    else
                    {
                        ReportError("Decimal overflow");
                        return Constant.Null;
                    }
                case TokenKind.IMAG_INT:
                    if (Int64.TryParse(CurToken.Value, out intRes))
                        return new Complex(new Integer(0), new Integer(intRes));
                    else
                    {
                        ReportError("Imaginary int overflow");
                        return Constant.Null;
                    }
                
                case TokenKind.IMAG_DEC:
                    if (decimal.TryParse(CurToken.Value, out decRes))
                        return new Complex(new Integer(0), new Irrational(decRes));
                    else
                    {
                        ReportError("Imaginary decimal overflow");
                        return Constant.Null;
                    }
                
                default:
                    throw new Exception("Wrong number token");
            }
        }
            
//        public Expression ParseFunction(string identifier)
//        {
//            List res = ParseList();
//            var args = res.Items;
//                
//            switch (identifier.ToLower())
//            {
//                case "abs":
//                    return new AbsFunc(CurScope);
//                case "sin":
//                    return new SinFunc(CurScope);
//                case "cos":
//                    return new CosFunc(CurScope);
//                case "tan":
//                    return new TanFunc(CurScope);
//                case "asin":
//                    return new AsinFunc(CurScope);
//                case "acos":
//                    return new AcosFunc(CurScope);
//                case "atan":
//                    return new AtanFunc(CurScope);
//                case "sqrt":
//                    return new SqrtFunc(CurScope);
//                case "reduce":
//                    return new ReduceFunc(CurScope);
//                case "expand":
//                    return new ExpandFunc(CurScope);
//                case "range":
//                    return new RangeFunc(CurScope);
//                case "solve":
//                    return new SolveFunc(CurScope);
//                case "type":
//                    return new TypeFunc(CurScope);
//                case "eval":
//                    return new EvalFunc(CurScope);
//                case "print":
//                    return new PrintFunc(CurScope);
//                case "plot":
//                    return new PlotFunc(CurScope);
//                case "paraplot":
//                    return new ParaPlotFunc(CurScope);
//                case "line":
//                    return new LineFunc(CurScope);
//            }
//        }

        public Error ReportError(Error error)
        {
            Errors.Add(error);

            return error;
        }

        public Error ReportError(string msg)
        {
            var error = new Error(msg);
            error.Position = CurToken.Position;

            Errors.Add(error);

            return error;
        }
    }
}

