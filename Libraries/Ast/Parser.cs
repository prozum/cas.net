using System;
using System.Collections.Generic;

namespace Ast
{

    public enum ParseContext
    {
        List,
        Scope
    }

    public class Parser
    {
        Scope scope;
        Token tok;
        Error _error;
        //Pos pos;
        ParseContext cx = ParseContext.Scope;

        Queue<Token> tokens;

        readonly Token EOS = new Token(TokenKind.END_OF_STRING, "END_OF_STRING", new Pos());

        public void Parse(string parseString)
        {
            Parse(parseString, new Scope());
        }

        public void Parse(string parseString, Scope global)
        {
            _error = null;
            scope = global;

            tokens = Scanner.Tokenize(parseString, out _error);
            if (_error != null)
            {
                scope.statements.Add(_error);
                return;
            }

            tok = tokens.Peek();
            ParseScope(TokenKind.END_OF_STRING);
        }

        public Scope ParseScope(TokenKind stopToken, bool newScope = false)
        {
            Scope res;

            if (newScope)
                res = scope = new Scope(scope);
            else
                res = scope;

            var lastCx = cx;
            cx = ParseContext.Scope;

            while (tokens.Count > 0)
            {
                if (Eat(stopToken))
                {
                    if (newScope)
                        scope = scope.parent;
                    cx = lastCx;
                    return res;
                }

                Peek();
                switch (tok.kind)
                {
                    case TokenKind.IF:
                        Eat();
                        scope.statements.Add(ParseIfStmt());
                        break;
                    case TokenKind.FOR:
                        Eat();
                        scope.statements.Add(ParseForStmt());
                        break;
                    case TokenKind.SEMICOLON:
                    case TokenKind.NEW_LINE:
                        Eat();
                        break;
                    default:
                        scope.statements.Add(ParseExpr(stopToken));
                        break;
                }

                if (_error != null)
                {
                    scope.statements.Add(_error);
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
            if (tokens.Count > 0 && tokens.Peek().kind == kind)
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
            if (tokens.Count > 0 && tokens.Peek().kind == kind)
            {
                tok = tokens.Dequeue();
                return true;
            }
                
            return false;
        }

        #endregion

        public IfStmt ParseIfStmt()
        {
            var stmt = new IfStmt();

            stmt.conditions.Add(ParseExpr(TokenKind.COLON));

            if (!Eat(TokenKind.COLON))
            {
                ReportSyntaxError("If syntax: if bool: {}");
                return null;
            }

            if (Eat(TokenKind.CURLY_START))
                stmt.expressions.Add(ParseScope(TokenKind.CURLY_END, true));
            else
            {
                stmt.expressions.Add(ParseScope(TokenKind.SEMICOLON, true));

                if (!Eat(TokenKind.SEMICOLON))
                {
                    ReportSyntaxError("If syntax: if bool: {}");
                    return null;
                }
            }

//            if (Eat())
//            {
//                ReportSyntaxError("If syntax: if bool: {}");
//                return null;
//            }

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
                    stmt.expressions.Add(ParseScope(TokenKind.CURLY_END, true));
                else
                    stmt.expressions.Add(ParseScope(TokenKind.SEMICOLON, true));
                //tokens.Dequeue(); // Skip '}' or ';'
            }

            if (Eat(TokenKind.ELSE))
            {

                if (Eat(TokenKind.CURLY_START))
                    stmt.expressions.Add(ParseScope(TokenKind.CURLY_END, true));
                else
                    stmt.expressions.Add(ParseScope(TokenKind.SEMICOLON, true));
                //tokens.Dequeue(); // Skip '}' or ';'
            }

            return stmt;
        }

        public ForStmt ParseForStmt()
        {
            var stmt = new ForStmt();

//            if (!Eat(TokenKind.IDENTIFIER))
//            {
//                ReportSyntaxError("If syntax: for symbol in list:");
//                return null;
//            }
//
//            if (!Eat(TokenKind.IN))
//            {
//                ReportSyntaxError("If syntax: for symbol in list:");
//                return null;
//            }
//
//            stmt.list = ParseExpr(TokenKind.COLON);

            throw new NotImplementedException();

        }

        public List ParseList()
        {
            var list = new List();

            var lastCx = cx;
            cx = ParseContext.List;

            while (tokens.Count > 0)
            {
                if (Eat(TokenKind.SQUARE_END))
                {
                    cx = lastCx;
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
            Expression expr = null;

            var exprs = new Queue<Expression>();
            var biops = new Queue<BinaryOperator>();
            var unops = new Queue<UnaryOperator>();

            bool isUnaryAllowed = true;

            while (tokens.Count > 0)
            {
                if (Peek(stopToken))
                    break;

                Eat(); // Get switch token

                switch (tok.kind)
                {
                    case TokenKind.SEMICOLON:
                        if (cx == ParseContext.Scope)
                            return CreateAst(exprs, biops);
                        else
                            return ReportSyntaxError("Unexpected ';' in list");
                    
                    case TokenKind.COMMA:
                        if (cx == ParseContext.List)
                            return CreateAst(exprs, biops);
                        else
                            return ReportSyntaxError("Unexpected ',' in scope");

                    case TokenKind.NEW_LINE:
                        return CreateAst(exprs, biops);

                    case TokenKind.INTEGER:
                    case TokenKind.DECIMAL:
                    case TokenKind.IMAG_INT:
                    case TokenKind.IMAG_DEC:
                        expr = ParseNumber();
                        break;
                    
                    case TokenKind.TEXT:
                        expr = new Text(tok.value);
                        break;
                    
                    case TokenKind.IDENTIFIER:
                        var sym = tok.value;
                        if (Eat(TokenKind.SQUARE_START))
                            expr = ParseFunction(sym);
                        else
                            expr = new Symbol(sym, scope);
                        break;

                    case TokenKind.TRUE:
                        expr = new Boolean(true);
                        break;
                    case TokenKind.FALSE:
                        expr = new Boolean(false);
                        break;

                    case TokenKind.ASSIGN:
                        biops.Enqueue(new Assign());
                        break;
                    case TokenKind.EQUAL:
                        biops.Enqueue(new Equal());
                        break;
                    case TokenKind.BOOL_EQUAL:
                        biops.Enqueue(new BooleanEqual());
                        break;
                    case TokenKind.NOT_EQUAL:
                        biops.Enqueue(new NotEqual());
                        break;
                    case TokenKind.LESS_EQUAL:
                        biops.Enqueue(new LesserEqual());
                        break;
                    case TokenKind.GREAT_EQUAL:
                        biops.Enqueue(new GreaterEqual());
                        break;
                    case TokenKind.LESS:
                        biops.Enqueue(new Lesser());
                        break;
                    case TokenKind.GREAT:
                        biops.Enqueue(new Greater());
                        break;

                    case TokenKind.ADD:
                        if (!isUnaryAllowed) // Ignore Unary +
                            biops.Enqueue(new Add());
                        break;
                    case TokenKind.SUB:
                        if (isUnaryAllowed)
                            unops.Enqueue(new Minus());
                        else
                            biops.Enqueue(new Sub());
                        break;

                    case TokenKind.MUL:
                        biops.Enqueue(new Mul());
                        break;
                    case TokenKind.DIV:
                        biops.Enqueue(new Div());
                        break;
                    case TokenKind.EXP:
                        biops.Enqueue(new Exp());
                        break;
                    case TokenKind.NEG:
                        if (isUnaryAllowed)
                            unops.Enqueue(new Negation());
                        else
                            ReportSyntaxError("Unexpected: '!'");
                        break;

                    case TokenKind.PARENT_START:
                        expr = ParseExpr(TokenKind.PARENT_END);
                        break;
                    case TokenKind.SQUARE_START:
                        expr = ParseList();
                        break;
                    case TokenKind.CURLY_START:
                        expr = ParseScope(TokenKind.CURLY_END, true);
                        break;

                    default:
                        return ReportSyntaxError("Unexpected '" + tok.ToString() + "'");
                }
                
                isUnaryAllowed = true;

                if (expr != null)
                {
                    expr.pos = tok.pos;

                    while (unops.Count > 0)
                    {
                        var unop = unops.Dequeue();
                        unop.child = expr;
                        expr = unop;
                    }

                    exprs.Enqueue(expr);
                    isUnaryAllowed = false;

                    expr = null;
                }

                if (exprs.Count != biops.Count && exprs.Count != biops.Count + 1)
                {
                    if (biops.Count > exprs.Count)
                        return ReportSyntaxError("Missing operand");
                    else
                        return ReportSyntaxError("Missing operator");
                }

                if (_error != null)
                    return _error;
            }

            return CreateAst(exprs, biops);
        }

        public Expression CreateAst(Queue<Expression> exprs, Queue<BinaryOperator> biops)
        {
            Expression left, right;
            BinaryOperator curOp, nextOp, top;

            if (exprs.Count == 0)
                throw new Exception("No expressions");

            if (exprs.Count != 1 + biops.Count)
                throw new Exception("Expressions != Binary Operators + 1");

            if (biops.Count >= exprs.Count)
                return ReportSyntaxError("Missing operand");

            if (exprs.Count == 1 && biops.Count == 0)
                return exprs.Dequeue();
                
                
            if (biops.Count > 0)
                top = biops.Peek();
            else
                return ReportSyntaxError("Missing operator");


            left = exprs.Dequeue();

            while (biops.Count > 0)
            {
                curOp = biops.Dequeue();
                curOp.Left = left;

                if (biops.Count > 0)
                {
                    nextOp = biops.Peek();

                    if (curOp.priority >= nextOp.priority)
                    {
                        right = exprs.Dequeue();
                        curOp.Right = right;

                        if (top.priority >= nextOp.priority)
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

            switch (tok.kind)
            {
                case TokenKind.INTEGER:
                    if (Int64.TryParse(tok.value, out intRes))
                        return new Integer(intRes);
                    else
                        return ReportSyntaxError("Int overflow");
                
                case TokenKind.DECIMAL:
                    if (decimal.TryParse(tok.value, out decRes))
                        return new Irrational(decRes);
                    else
                        return ReportSyntaxError("Decimal overflow");
                
                case TokenKind.IMAG_INT:
                    if (Int64.TryParse(tok.value, out intRes))
                        return new Complex(new Integer(0), new Integer(intRes));
                    else
                        return ReportSyntaxError("Imaginary int overflow");
                
                case TokenKind.IMAG_DEC:
                    if (decimal.TryParse(tok.value, out decRes))
                        return new Complex(new Integer(0), new Irrational(decRes));
                    else
                        return ReportSyntaxError("Imaginary decimal overflow");
                
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
                    return new SinFunc(args, scope);
                case "cos":
                    return new CosFunc(args, scope);
                case "tan":
                    return new TanFunc(args, scope);
                case "asin":
                    return new AsinFunc(args, scope);
                case "acos":
                    return new AcosFunc(args, scope);
                case "atan":
                    return new AtanFunc(args, scope);
                case "sqrt":
                    return new SqrtFunc(args, scope);
                case "reduce":
                    return new ReduceFunc(args, scope);
                case "expand":
                    return new ExpandFunc(args, scope);
                case "range":
                    return new RangeFunc(args, scope);
                case "map":
                    return new MapFunc(args, scope);
                case "plot":
                    return new PlotFunc(args, scope);
                case "solve":
                    return new SolveFunc(args, scope);
                case "enter":
                    return new EnterFunc(args, scope);
                case "exit":
                    return new ExitFunc(args, scope);
                case "print":
                    return new PrintFunc(args, scope);
                case "type":
                    return new TypeFunc(args, scope);
                case "eval":
                    return new EvalFunc(args, scope);
                default:
                    return new UsrFunc(sym.ToLower(), args, scope);
            }
        }

        public Error ReportSyntaxError(string msg)
        {
            _error = new Error("Parser: " + msg);
            _error.pos = tok.pos;

            return _error;
        }
    }
}

