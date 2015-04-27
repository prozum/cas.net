using System;
using System.Collections.Generic;

namespace Ast
{
    public static class Parser
    {
        static Scope scope;
        static Error error;
        static Pos pos;

        public static Scope Parse(string parseString)
        {
            return Parse(parseString, new Scope());
        }

        public static Scope Parse(string parseString, Scope globalScope)
        {
            error = null;
            scope = globalScope;
            return ParseScope(Scanner.Tokenize(parseString), TokenKind.END_OF_STRING, false);
        }

        public static Scope ParseScope(Queue<Token> tokens, TokenKind stopToken, bool newScope = false)
        {
            Scope res;

            Token tok;

            if (newScope)
                res = scope = new Scope(scope);
            else
                res = scope;

            while (tokens.Count > 0)
            {
                tok = tokens.Peek();

                if (tok.kind == stopToken)
                {
                    break;
                }

                if (tok.kind == TokenKind.IF)
                {
                    scope.statements.Add(ParseIfState(tokens));
                }
                else
                {
                    scope.statements.Add(ParseExprState(tokens, stopToken));
                }
            }

            if (newScope)
                scope = scope.parent;

            return res;
        }

        public static ExprState ParseExprState(Queue<Token> tokens, TokenKind stopToken)
        {
            var state = new ExprState();


            state.expr = ParseExpr(tokens, stopToken);

            return state;
        }

        public static IfState ParseIfState(Queue<Token> tokens)
        {
            var state = new IfState();

            tokens.Dequeue(); // Skip 'if'

            state.conditions.Add(ParseExpr(tokens, TokenKind.COLON));
            tokens.Dequeue(); // Skip ':'

            if (tokens.Count == 0)
            {
                ErrorHandler("If syntax: if bool expr: {}");
                return null;
            }

            if (tokens.Peek().kind == TokenKind.CURLY_START)
                state.expressions.Add(ParseScope(tokens, TokenKind.CURLY_END, true));
            else
                state.expressions.Add(ParseScope(tokens, TokenKind.SEMICOLON, true));
            tokens.Dequeue(); // Skip '}' or ';'
               

            while (tokens.Count > 0 && tokens.Peek().kind == TokenKind.ELIF)
            {
                tokens.Dequeue(); // Skip 'elif'

                state.conditions.Add(ParseExpr(tokens, TokenKind.COLON));
                tokens.Dequeue(); // Skip ':'

                if (tokens.Peek().kind == TokenKind.CURLY_START)
                    state.expressions.Add(ParseScope(tokens, TokenKind.CURLY_END, true));
                else
                    state.expressions.Add(ParseScope(tokens, TokenKind.SEMICOLON, true));
                tokens.Dequeue(); // Skip '}' or ';'
            }

            if (tokens.Count > 0 && tokens.Peek().kind == TokenKind.ELSE)
            {
                tokens.Dequeue(); // Skip 'else'

                if (tokens.Peek().kind == TokenKind.CURLY_START)
                    state.expressions.Add(ParseScope(tokens, TokenKind.CURLY_END, true));
                else
                    state.expressions.Add(ParseScope(tokens, TokenKind.SEMICOLON, true));
                tokens.Dequeue(); // Skip '}' or ';'
            }

            return state;
        }

        public static Expression ParseExpr(Queue<Token> tokens, TokenKind stopToken, bool list = false)
        {
            Token tok;
            List resList = null;

            Expression expr = null;

            var exs = new Queue<Expression> ();
            var ops = new Queue<Operator> ();

            bool first = true;

            if (list)
                resList = new List();

            while (tokens.Count > 0)
            {
                if (tokens.Peek().kind == stopToken)
                    break;

                tok = tokens.Dequeue();
                pos = tok.pos;

                switch (tok.kind)
                {
                    case TokenKind.INTEGER:
                    case TokenKind.DECIMAL:
                    case TokenKind.IMAG_INT:
                    case TokenKind.IMAG_DEC:
                        expr = ParseNumber(tok);
                        break;
                    
                    case TokenKind.TEXT:
                        expr = new Text(tok.value);
                        break;
                    
                    case TokenKind.IDENTIFIER:
                        if (tokens.Count > 0 && tokens.Peek().kind == TokenKind.SQUARE_START)
                        {
                            expr = ParseFunction(tok, tokens);
                        }
                        else
                        {
                            expr = new Symbol(tok.value, scope);
                        }
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
                            expr = ParseNumber(tokens.Dequeue(), true);
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
                        expr = ParseExpr(tokens, TokenKind.PARENT_END);
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ErrorHandler("Missing ) bracket");
                        break;
                    case TokenKind.SQUARE_START:
                        expr = ParseExpr(tokens, TokenKind.SQUARE_END, true);
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ErrorHandler("Missing ] bracket");
                        break;
                    case TokenKind.CURLY_START:
                        exs.Enqueue(ParseScope(tokens, TokenKind.CURLY_END, true));
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ErrorHandler("Missing } bracket");
                        break;
                    case TokenKind.COMMA:
                        if (list)
                            resList.items.Add(CreateAst(exs, ops));
                        else
                            expr = ErrorHandler("Invalid comma");
                        break;
                    case TokenKind.SEMICOLON:
                        if (!list)
                            return CreateAst(exs, ops);
                        else
                            expr = ErrorHandler("Unexpected semicolon in list");
                        break;

                    case TokenKind.PARENT_END:
                    case TokenKind.SQUARE_END:
                    case TokenKind.CURLY_END:
                        expr = ErrorHandler("Unexpected end bracket");
                        break;
                    case TokenKind.UNKNOWN:
                        expr = ErrorHandler("Unknown token");
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

            if (list)
            {
                if (exs.Count > 0)
                    resList.items.Add(CreateAst(exs, ops));
                return resList;
            }

            return CreateAst(exs, ops);
        }

        public static Expression CreateAst(Queue<Expression> exs, Queue<Operator> ops)
        {
            Expression left, right;
            Operator curOp, nextOp, top;

            if (exs.Count == 0)
                return ErrorHandler("No expressions");
            if (exs.Count == 1 && ops.Count == 0)
                return exs.Dequeue();
                
            if (ops.Count > 0)
                top = ops.Peek();
            else
                return ErrorHandler("Missing operator");


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
                    if (exs.Count <= 0)
                        return ErrorHandler("Missing right operand");
                    right = exs.Dequeue();
                    curOp.Right = right;
                }
            }

            if (exs.Count != 0)
                return ErrorHandler("The operators cannot use all the operands");

            return top;
        }

        public static Expression ParseNumber(Token tok, bool negative = false)
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
                        return ErrorHandler("Int overflow");
                case TokenKind.DECIMAL:
                    if (decimal.TryParse(tok.value, out decRes))
                    {
                        if (negative)
                            return new Irrational(-decRes);
                        else
                            return new Irrational(decRes);
                    }
                    else
                        return ErrorHandler("Decimal overflow");
                case TokenKind.IMAG_INT:
                    if (Int64.TryParse(tok.value, out intRes))
                    {
                        if (negative)
                            return new Complex(new Integer(0), new Integer(-intRes));
                        else
                            return new Complex(new Integer(0), new Integer(intRes));
                    }
                    else
                        return ErrorHandler("Imaginary int overflow");
                case TokenKind.IMAG_DEC:
                    if (decimal.TryParse(tok.value, out decRes))
                    {
                        if (negative)
                            return new Complex(new Integer(0), new Irrational(-decRes));
                        else
                            return new Complex(new Integer(0), new Irrational(decRes));
                    }
                    else
                        return ErrorHandler("Imaginary decimal overflow");
                default:
                    throw new Exception("Wrong number token");
            }
        }


        public static Expression ParseFunction(Token tok, Queue<Token> tokens)
        {
            List<Expression> args;

            tokens.Dequeue(); // Eat start parentheses
            Expression res = ParseExpr(tokens, TokenKind.SQUARE_END, true);
            if (tokens.Count > 0)
                tokens.Dequeue();
            else
                ErrorHandler("Missing ] bracket");

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
        public static Error ErrorHandler(string msg)
        {
            error = new Error("Parser at [" + pos.Column + ";" + pos.Line + "]: " + msg);

            return error;
        }
    }
}

