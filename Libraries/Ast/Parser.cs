using System;
using System.Collections.Generic;

namespace Ast
{
    public static class Parser
    {
        static Scope scope;
        static Error error;

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
                    scope.statements.Enqueue(ParseExpr(tokens, stopToken));
            }

            if (newScope)
                scope = scope.parent;

            return res;
        }

        public static Expression ParseExpr(Queue<Token> tokens, TokenKind stopToken, bool list = false)
        {
            Token tok;
            List resList = null;

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

                switch (tok.kind)
                {
                    case TokenKind.Integer:
                    case TokenKind.Decimal:
                    case TokenKind.ImaginaryInt:
                    case TokenKind.ImaginaryDec:
                        exs.Enqueue(ParseNumber(tok));
                        break;

                    case TokenKind.Identifier:
                        if (tokens.Count > 0 && tokens.Peek().kind == TokenKind.SquareStart)
                        {
                            tokens.Dequeue(); // Eat start parentheses
                            exs.Enqueue(ParseFunction(tok, tokens));
                        }
                        else
                            exs.Enqueue(new Symbol(tok.value, scope));
                        break;
                    case TokenKind.KW_True:
                        exs.Enqueue(new Boolean(true));
                        break;
                    case TokenKind.KW_False:
                        exs.Enqueue(new Boolean(false));
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
                    case TokenKind.LesserOrEqual:
                        ops.Enqueue(new LesserOrEqual());
                        break;
                    case TokenKind.GreaterOrEqual:
                        ops.Enqueue(new GreaterOrEqual());
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
                            exs.Enqueue(ParseNumber(tokens.Dequeue(), true));
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
                    
                    case TokenKind.ParenthesesStart:
                        exs.Enqueue(ParseExpr(tokens, TokenKind.ParenthesesEnd));
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            ErrorHandler("Missing ) bracket");
                        break;
                    case TokenKind.SquareStart:
                        exs.Enqueue(ParseExpr(tokens, TokenKind.SquareEnd, true));
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            ErrorHandler("Missing ] bracket");
                        break;
                    case TokenKind.CurlyStart:
                        exs.Enqueue(ParseScope(tokens, TokenKind.CurlyEnd, true));
                        if (tokens.Count > 0)
                            tokens.Dequeue();
                        else
                            ErrorHandler("Missing } bracket");
                        break;
                    case TokenKind.Comma:
                        if (list)
                            resList.elements.Add(CreateAst(exs, ops));
                        else
                            return new Error("Invalid comma");
                        break;
                    case TokenKind.Semicolon:
                        if (!list)
                            return CreateAst(exs, ops);
                        else
                            ErrorHandler("Unexpected semicolon in list");
                        break;

                    case TokenKind.ParenthesesEnd:
                    case TokenKind.SquareEnd:
                    case TokenKind.CurlyEnd:
                        ErrorHandler("Unexpected end bracket");
                        break;
                    case TokenKind.Unknown:
                        ErrorHandler("Unknown token");
                        break;
                }

                first = false;

                if (error != null)
                    return error;
            }

            if (list)
            {
                resList.elements.Add(CreateAst(exs, ops));
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
            if (exs.Count == 1)
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

            Expression res = ParseExpr(tokens, TokenKind.SquareEnd, true);
            if (tokens.Count > 0)
                tokens.Dequeue();
            else
                ErrorHandler("Missing ) bracket");

            if (res is List)
                args = (res as List).elements;
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
                default:
                    return new InstanceFunc(tok.value, args, scope);
                    //res = new UsrFunc(tok.value.ToLower(), args, scope);
            }

            return res;

        }
        public static Error ErrorHandler(string message)
        {
            error = new Error(message);

            return error;
        }
    }
}

