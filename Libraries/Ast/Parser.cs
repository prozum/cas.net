using System;
using System.Collections.Generic;

namespace Ast
{
    public static class Parser
    {
        static Scope scope;
        //Scanner scanner;
        //Evaluator evaluator;
        static Error error;

        //static Queue<Expression> exs;
        //static Queue<Operator> ops;

//        public Parser() : this(new Evaluator()) { }
//        public Parser (Evaluator eval)
//        {
//            this.scanner = new Scanner();
//            this.evaluator = eval;
//        }

        public static Expression Parse(string parseString, Scope parseScope)
        {
            error = null;
            scope = parseScope;
            return Parse(Scanner.Tokenize(parseString), TokenKind.EndOfString);
        }

        public static Expression Parse(Queue<Token> tokens, TokenKind stopToken, bool list = false)
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
                tok = tokens.Dequeue();

                if (tok.kind == stopToken)
                    break;

                switch (tok.kind)
                {
                    case TokenKind.Integer:
                    case TokenKind.Decimal:
                    case TokenKind.ImaginaryInt:
                    case TokenKind.ImaginaryDec:
                        exs.Enqueue(ParseNumber(tok));
                        break;

                    case TokenKind.Identifier:
                        if (tokens.Count > 0 && tokens.Peek().kind == TokenKind.ParenthesesStart)
                        {
                            tokens.Dequeue(); // Eat start parentheses
                            exs.Enqueue(ParseFunction(tok, tokens));
                        }
                        else
                            exs.Enqueue(new Symbol(tok.value, scope));
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
                        exs.Enqueue(Parse(tokens, TokenKind.ParenthesesEnd, true));
                        break;
                    case TokenKind.SquareStart:
                        exs.Enqueue(Parse(tokens, TokenKind.SquareEnd, true));
                        break;
                    case TokenKind.CurlyStart:
                        exs.Enqueue(Parse(tokens, TokenKind.CurlyEnd, true));
                        break;
                    case TokenKind.Comma:
                        if (resList == null)
                            resList.elements.Add(CreateAst(exs, ops));
                        else
                            return new Error("Invalid comma");
                        //ExtractBrackets(tok.kind, tokens, TokenKind.Comma);
                        //exs.Enqueue(ExtractBrackets(tok.kind, tokens, TokenKind.None)[0]);
                        //break;
                        //var list = new List();
                        //list.elements = ExtractBrackets(tok.kind, tokens, TokenKind.Semicolon);
                        //exs.Enqueue(list);
                        break;
                    case TokenKind.ParenthesesEnd:
                    case TokenKind.SquareEnd:
                    case TokenKind.CurlyEnd:
                        ErrorHandler("Unexpected end bracker");
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

//        public static void ExtractBrackets(TokenKind startBracket, Queue<Token> tokens, TokenKind seperator)
//        {
//            //List<Expression> exs = new List<Expression> ();
//            Token tok;
//
//            TokenKind endBracket;
//            switch (startBracket)
//            {
//                case TokenKind.ParenthesesStart:
//                    endBracket = TokenKind.ParenthesesEnd;
//                    break;
//                case TokenKind.SquareStart:
//                    endBracket = TokenKind.SquareEnd;
//                    break;
//                case TokenKind.CurlyStart:
//                    endBracket = TokenKind.CurlyEnd;
//                    break;
//                default:
//                    throw new Exception("Wrong bracket token");
//            }
//
//            var subTokens = new Queue<Token>();
//
//            while (tokens.Count > 0)
//            {
//                tok = tokens.Dequeue();
//
//                if (tok.kind == startBracket)
//                {
//                    ExtractBrackets(tok.kind, tokens, TokenKind.Comma);
//                }
//                else if (tok.kind == seperator)
//                {
//                    exs.Enqueue(Parse(subTokens, endBracket));
//                }
//                else if (tok.kind == endBracket)
//                {
//                    exs.Enqueue(Parse(subTokens));
//                    return;
//                }
//                else
//                {
//                    subTokens.Enqueue(tok);
//                }
//            }
//
//            error = new Error("Missing end bracket");
//        }

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

//        public static Expression ExtractArgs(TokenKind startBracket, Queue<Token> tokens, TokenKind seperator)
//        {
//            List<Expression> args = new List<Expression> ();
//
//            Token tok;
//
//            TokenKind endBracket;
//            switch (startBracket)
//            {
//                case TokenKind.ParenthesesStart:
//                    endBracket = TokenKind.ParenthesesEnd;
//                    break;
//                case TokenKind.SquareStart:
//                    endBracket = TokenKind.SquareEnd;
//                    break;
//                case TokenKind.CurlyStart:
//                    endBracket = TokenKind.CurlyEnd;
//                    break;
//                default:
//                    throw new Exception("Wrong bracket token");
//            }
//
//            var subTokens = new Queue<Token>();
//
//            while (tokens.Count > 0)
//            {
//                tok = tokens.Dequeue();
//
//                if (tok.kind == startBracket)
//                {
//                    ExtractBrackets(tok.kind, tokens, TokenKind.Comma);
//                }
//                else if (tok.kind == seperator)
//                {
//                    exs.Enqueue(Parse(subTokens));
//                }
//                else if (tok.kind == endBracket)
//                {
//                    exs.Enqueue(Parse(subTokens));
//                    return null;
//                }
//                else
//                {
//                    subTokens.Enqueue(tok);
//                }
//            }
//
//            error = new Error("Missing end bracket");
//        }

        public static Expression ParseFunction(Token tok, Queue<Token> tokens)
        {
            scope = new Scope(scope);

            var args = Parse(tokens, TokenKind.ParenthesesEnd, true);

            scope = scope.parent;

            throw new NotImplementedException();
            //return new Func(tok.value.ToLower(), args, scope);
//            switch (tok.value.ToLower())
//            {
//                case "sin":
//                    res = new Sin(args, evaluator);
//                    break;
//                case "cos":
//                    res = new Cos(args, evaluator);
//                    break;
//                case "tan":
//                    res = new Tan(args, evaluator);
//                    break;
//                case "asin":
//                    res = new ASin(args, evaluator);
//                    break;
//                case "acos":
//                    res = new ACos(args, evaluator);
//                    break;
//                case "atan":
//                    res = new ATan(args, evaluator);
//                    break;
//                case "sqrt":
//                    res = new Sqrt(args, evaluator);
//                    break;
//                case "simplify":
//                    res = new Simplify(args, evaluator);
//                    break;
//                case "expand":
//                    res = new Expand(args, evaluator);
//                    break;
//                case "range":
//                    res = new Range(args, evaluator);
//                    break;
//                case "map":
//                    res = new Map(args, evaluator);
//                    break;
//                case "plot":
//                    res = new Plot(args, evaluator);
//                    break;
//                case "solve":
//                    res = new Solve(args, evaluator);
//                    break;
//                default:
//                    res = new UsrFunc(tok.value.ToLower(), args, evaluator);
//                    break;
//            }

//            return res;

        }
        public static Error ErrorHandler(string message)
        {
            error = new Error(message);

            return error;
        }
    }
}

