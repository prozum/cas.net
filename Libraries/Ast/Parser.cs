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
            return ParseScope(Scanner.Tokenize(parseString), TokenKind.EndOfString, false);
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
                else
                    scope.statements.Add(ParseExpr(tokens, stopToken));
            }

            if (newScope)
                scope = scope.parent;

            return res;
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
                    case TokenKind.Integer:
                    case TokenKind.Decimal:
                    case TokenKind.ImaginaryInt:
                    case TokenKind.ImaginaryDec:
                        expr = ParseNumber(tok);
                        break;

                    case TokenKind.Identifier:
                        if (tokens.Count > 0 && tokens.Peek().kind == TokenKind.SquareStart)
                        {
                            expr = ParseFunction(tok, tokens);
                        }
                        else
                        {
                            expr = new Symbol(tok.value, scope);
                        }
                        break;
                    case TokenKind.KW_True:
                        expr = new Boolean(true);
                        break;
                    case TokenKind.KW_False:
                        expr = new Boolean(false);
                        break;

                    case TokenKind.Assign:
                        ops.Enqueue(new Assign());
                        break;
                    case TokenKind.Equal:
                        ops.Enqueue(new Equal());
                        break;
                    case TokenKind.BooleanEqual:
                        ops.Enqueue(new BooleanEqual());
                        break;
                    case TokenKind.LesserEqual:
                        ops.Enqueue(new LesserEqual());
                        break;
                    case TokenKind.GreaterEqual:
                        ops.Enqueue(new GreaterEqual());
                        break;
                    case TokenKind.Lesser:
                        ops.Enqueue(new Lesser());
                        break;
                    case TokenKind.Greater:
                        ops.Enqueue(new Greater());
                        break;
                    case TokenKind.Add:
                        ops.Enqueue(new Add());
                        break;
                    case TokenKind.Sub:
                        if (first)
                            expr = ParseNumber(tokens.Dequeue(), true);
                        else
                            ops.Enqueue(new Sub());
                        break;
                    case TokenKind.Mul:
                        ops.Enqueue(new Mul());
                        break;
                    case TokenKind.Div:
                        ops.Enqueue(new Div());
                        break;
                    case TokenKind.Exp:
                        ops.Enqueue(new Exp());
                        break;
                    
                    case TokenKind.ParentStart:
                        expr = ParseExpr(tokens, TokenKind.ParentEnd);
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ErrorHandler("Missing ) bracket");
                        break;
                    case TokenKind.SquareStart:
                        expr = ParseExpr(tokens, TokenKind.SquareEnd, true);
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ErrorHandler("Missing ] bracket");
                        break;
                    case TokenKind.CurlyStart:
                        exs.Enqueue(ParseScope(tokens, TokenKind.CurlyEnd, true));
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            expr = ErrorHandler("Missing } bracket");
                        break;
                    case TokenKind.Comma:
                        if (list)
                            resList.items.Add(CreateAst(exs, ops));
                        else
                            expr = ErrorHandler("Invalid comma");
                        break;
                    case TokenKind.Semicolon:
                        if (!list)
                            return CreateAst(exs, ops);
                        else
                            expr = ErrorHandler("Unexpected semicolon in list");
                        break;

                    case TokenKind.ParentEnd:
                    case TokenKind.SquareEnd:
                    case TokenKind.CurlyEnd:
                        expr = ErrorHandler("Unexpected end bracket");
                        break;
                    case TokenKind.Unknown:
                        expr = ErrorHandler("Unknown token");
                        break;
                }
                
                first = false;


                if (expr != null)
                {
                    pos = tok.pos;
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
                    if (!(exs.Count > 0))
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
                case TokenKind.Integer:
                    if (Int64.TryParse(tok.value, out intRes))
                    {
                        if (negative)
                            return new Integer(-intRes);
                        else
                            return new Integer(intRes);
                    }
                    else
                        return ErrorHandler("Int overflow");
                case TokenKind.Decimal:
                    if (decimal.TryParse(tok.value, out decRes))
                    {
                        if (negative)
                            return new Irrational(-decRes);
                        else
                            return new Irrational(decRes);
                    }
                    else
                        return ErrorHandler("Decimal overflow");
                case TokenKind.ImaginaryInt:
                    if (Int64.TryParse(tok.value, out intRes))
                    {
                        if (negative)
                            return new Complex(new Integer(0), new Integer(-intRes));
                        else
                            return new Complex(new Integer(0), new Integer(intRes));
                    }
                    else
                        return ErrorHandler("Imaginary int overflow");
                case TokenKind.ImaginaryDec:
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
            Expression res = ParseExpr(tokens, TokenKind.SquareEnd, true);
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

