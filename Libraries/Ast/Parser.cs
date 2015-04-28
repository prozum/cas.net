using System;
using System.Collections.Generic;

namespace Ast
{

    public enum Context
    {
        Expr,
        List,
        Scope
    }

    public class Parser
    {
        Scope scope;
        Token tok;
        Error _error;
        Pos pos;
        Context cx;

        Queue<Token> tokens;

        readonly Token EOS = new Token(TokenKind.END_OF_STRING, "END_OF_STRING", pos);

        public Scope Parse(string parseString)
        {
            return Parse(parseString, new Scope());
        }

        public Scope Parse(string parseString, Scope global)
        {
            _error = null;
            scope = global;
            tokens = Scanner.Tokenize(parseString);
            return ParseScope(TokenKind.END_OF_STRING, false);
        }

        public Scope ParseScope(TokenKind stopToken, bool newScope = false)
        {
            Scope res;
            Statement stmt;

            if (newScope)
                res = scope = new Scope(scope);
            else
                res = scope;

            while (tokens.Count > 0)
            {
                if (Eat(stopToken))
                    break;

                if (Eat(TokenKind.IF))
                    stmt = ParseIfStmt();
                else if (Eat(TokenKind.FOR))
                    stmt = ParseForStmt();
                else
                    stmt = new ExprStmt(ParseExpr(stopToken));

                if (_error != null)
                    stmt = new ExprStmt(_error);

                scope.statements.Add(stmt);
            }

            if (newScope)
                scope = scope.parent;

            return res;
        }

        #region Peek Eat

        public bool Peek()
        {
            if (tok.kind == TokenKind.END_OF_STRING)
                return false;

            tok = tokens.Peek();
            return true;
        }

        public bool Peek(TokenKind kind)
        {
            if (tok.kind != TokenKind.END_OF_STRING)
                tok = tokens.Peek();

            return tok.kind == kind;
        }

        public bool Eat()
        {
            if (tok.kind != TokenKind.END_OF_STRING)
                tok = tokens.Dequeue();

            return tok.kind != TokenKind.END_OF_STRING;
        }

        public bool Eat(TokenKind kind)
        {
            if (tokens.Peek().kind == kind)
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

            while ();

        }

        public Expression ParseExpr(TokenKind stopToken)
        {
            Expression expr = null;

            var exs = new Queue<Expression> ();
            var ops = new Queue<Operator> ();

            bool first = true;

            while (tokens.Count > 0)
            {
                if (Eat(stopToken))
                    break;

                tok = tokens.Dequeue();
                pos = tok.pos;

                switch (tok.kind)
                {
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
                        if (Eat(TokenKind.SQUARE_START))
                            expr = ParseFunction();
                        else
                            expr = new Symbol(tok.value, scope);
                        break;

                    case TokenKind.TRUE:
                        expr = new Boolean(true);
                        break;
                    case TokenKind.FALSE:
                        expr = new Boolean(false);
                        break;

                    case TokenKind.ASSIGN:
                        ops.Enqueue(new Assign());
                        break;
                    case TokenKind.EQUAL:
                        ops.Enqueue(new Equal());
                        break;
                    case TokenKind.BOOL_EQUAL:
                        ops.Enqueue(new BooleanEqual());
                        break;
                    case TokenKind.LESS_EQUAL:
                        ops.Enqueue(new LesserEqual());
                        break;
                    case TokenKind.GREAT_EQUAL:
                        ops.Enqueue(new GreaterEqual());
                        break;
                    case TokenKind.LESS:
                        ops.Enqueue(new Lesser());
                        break;
                    case TokenKind.GREAT:
                        ops.Enqueue(new Greater());
                        break;
                    case TokenKind.ADD:
                        ops.Enqueue(new Add());
                        break;
                    case TokenKind.SUB:
                        if (first)
                            expr = ParseNumber(true);
                        else
                            ops.Enqueue(new Sub());
                        break;
                    case TokenKind.MUL:
                        ops.Enqueue(new Mul());
                        break;
                    case TokenKind.DIV:
                        ops.Enqueue(new Div());
                        break;
                    case TokenKind.EXP:
                        ops.Enqueue(new Exp());
                        break;
                    
                    case TokenKind.PARENT_START:
                        expr = ParseExpr(TokenKind.PARENT_END);
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ReportSyntaxError("Missing ) bracket");
                        break;
                    case TokenKind.SQUARE_START:
                        expr = ParseList();
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ReportSyntaxError("Missing ] bracket");
                        break;
                    case TokenKind.CURLY_START:
                        exs.Enqueue(ParseScope(TokenKind.CURLY_END, true));
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ReportSyntaxError("Missing } bracket");
                        break;
                    case TokenKind.COMMA:
                        if (cx == Context.List)
                            return CreateAst(exs, ops);
                        else
                            expr = ReportSyntaxError("Unexpected comma in scope");
                        break;
                    case TokenKind.SEMICOLON:
                        if (cx == Context.Scope)
                            return CreateAst(exs, ops);
                        else
                            expr = ReportSyntaxError("Unexpected semicolon in list");
                        break;

                    case TokenKind.PARENT_END:
                    case TokenKind.SQUARE_END:
                    case TokenKind.CURLY_END:
                        expr = ReportSyntaxError("Unexpected end bracket");
                        break;
                    case TokenKind.UNKNOWN:
                        expr = ReportSyntaxError("Unknown token");
                        break;
                }
                
                first = false;


                if (expr != null)
                {
                    pos = expr.pos = tok.pos;
                    exs.Enqueue(expr);
                    if (expr is Error)
                        return expr;
                    expr = null;
                }
            }

//            if (list)
//            {
//                if (exs.Count > 0)
//                    resList.items.Add(CreateAst(exs, ops));
//                return resList;
//            }

            return CreateAst(exs, ops);
        }

        public Expression CreateAst(Queue<Expression> exs, Queue<Operator> ops)
        {
            Expression left, right;
            Operator curOp, nextOp, top;

            if (exs.Count == 0)
                return ReportSyntaxError("No expressions");
            if (exs.Count == 1 && ops.Count == 0)
                return exs.Dequeue();
                
            if (ops.Count > 0)
                top = ops.Peek();
            else
                return ReportSyntaxError("Missing operator");


            left = exs.Dequeue();

            while (ops.Count > 0)
            {
                curOp = ops.Dequeue();
                curOp.Left = left;

                if (ops.Count > 0)
                {
                    nextOp = ops.Peek();

                    if (curOp.priority >= nextOp.priority)
                    {
                        right = exs.Dequeue();
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
                        left = exs.Dequeue();

                        right = nextOp;
                        curOp.Right = right;
                    }
                }
                else
                {
                    if (exs.Count == 0)
                        return ReportSyntaxError("Missing right operand");
                    right = exs.Dequeue();
                    curOp.Right = right;
                }
            }

            if (exs.Count != 0)
                return ReportSyntaxError("The operators cannot use all the operands");

            return top;
        }

        public Expression ParseNumber(bool negative = false)
        {
            Int64 intRes;
            decimal decRes;

            switch (tok.kind)
            {
                case TokenKind.INTEGER:
                    if (Int64.TryParse(tok.value, out intRes))
                    {
                        if (negative)
                            return new Integer(-intRes);
                        else
                            return new Integer(intRes);
                    }
                    else
                        return ReportSyntaxError("Int overflow");
                case TokenKind.DECIMAL:
                    if (decimal.TryParse(tok.value, out decRes))
                    {
                        if (negative)
                            return new Irrational(-decRes);
                        else
                            return new Irrational(decRes);
                    }
                    else
                        return ReportSyntaxError("Decimal overflow");
                case TokenKind.IMAG_INT:
                    if (Int64.TryParse(tok.value, out intRes))
                    {
                        if (negative)
                            return new Complex(new Integer(0), new Integer(-intRes));
                        else
                            return new Complex(new Integer(0), new Integer(intRes));
                    }
                    else
                        return ReportSyntaxError("Imaginary int overflow");
                case TokenKind.IMAG_DEC:
                    if (decimal.TryParse(tok.value, out decRes))
                    {
                        if (negative)
                            return new Complex(new Integer(0), new Irrational(-decRes));
                        else
                            return new Complex(new Integer(0), new Irrational(decRes));
                    }
                    else
                        return ReportSyntaxError("Imaginary decimal overflow");
                default:
                    throw new Exception("Wrong number token");
            }
        }


        public Expression ParseFunction()
        {
            List<Expression> args;

            List res = ParseList();

            //if (!Eat(TokenKind.SQUARE_END))
            //    ReportSyntaxError("Missing ] bracket");

            if (res is List)
                args = (res as List).items;
            else
                return res;
                
            switch (tok.value.ToLower())
            {
                case "sin":
                    return new Sin(args, scope);
                case "cos":
                    return new Cos(args, scope);
                case "tan":
                    return new Tan(args, scope);
                case "asin":
                    return new ASin(args, scope);
                case "acos":
                    return new ACos(args, scope);
                case "atan":
                    return new ATan(args, scope);
                case "sqrt":
                    return new Sqrt(args, scope);
                case "simplify":
                    return new Simplify(args, scope);
                case "expand":
                    return new Expand(args, scope);
                case "range":
                    return new Range(args, scope);
                case "map":
                    return new Map(args, scope);
                case "plot":
                    return new Plot(args, scope);
                case "solve":
                    return new Solve(args, scope);
                case "enter":
                    return new Enter(args, scope);
                case "Exit":
                    return new Exit(args, scope);
                default:
                    return new UsrFunc(tok.value.ToLower(), args, scope);
            }
        }

        public Error ReportSyntaxError(string msg)
        {
            _error = new Error("Parser at [" + tok.pos.Column + ";" + tok.pos.Line + "]: " + msg);

            return _error;
        }
    }
}

