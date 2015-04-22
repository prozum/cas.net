using System;
using System.Collections.Generic;

namespace Ast
{
    public class Parser
    {
        Scanner scanner;
        Evaluator evaluator;
        Error error;

        public Parser() : this(new Evaluator()) { }
        public Parser (Evaluator eval)
        {
            this.scanner = new Scanner();
            this.evaluator = eval;
        }

        public Expression Parse(string parseString)
        {
            return Parse(scanner.Tokenize(parseString));
        }

        public Expression Parse(Queue<Token> tokens)
        {
            var exs = new Queue<Expression> ();
            var ops = new Queue<Operator> ();
            bool first = true;
            error = null;

            while (tokens.Count > 0)
            {
                var tok = tokens.Dequeue();
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
                            exs.Enqueue(ParseFunction(tok, tokens));
                        else
                            exs.Enqueue(new Symbol(evaluator, tok.value));
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
                            exs.Enqueue(ParseNumber(tokens.Dequeue(),true));
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
                        exs.Enqueue(ExtractBrackets(tok.kind, tokens, TokenKind.None)[0]);
                        break;
                    case TokenKind.SquareStart:
                    case TokenKind.CurlyStart:
                        var list = new List();
                        list.elements = ExtractBrackets(tok.kind, tokens, TokenKind.Comma);
                        exs.Enqueue(list);
                        break;
                }
                first = false;

                if (error != null)
                    return error;
            }

            return CreateAst(exs, ops);
        }

        public Expression CreateAst(Queue<Expression> exs, Queue<Operator> ops)
        {
            Expression left,right;
            Operator curOp, nextOp,top;

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

            return top;
        }

        public List<Expression> ExtractBrackets(TokenKind startBracket, Queue<Token> tokens, TokenKind seperator)
        {
            List<Expression> exs = new List<Expression> ();

            Token tok;

            int start = 1;
            int end = 0;

            TokenKind endBracket;
            switch (startBracket)
            {
                case TokenKind.ParenthesesStart:
                    endBracket = TokenKind.ParenthesesEnd;
                    break;
                case TokenKind.SquareStart:
                    endBracket = TokenKind.SquareEnd;
                    break;
                case TokenKind.CurlyStart:
                    endBracket = TokenKind.CurlyEnd;
                    break;
                default:
                    throw new Exception("Wrong bracket token");
            }

            var subTokens = new Queue<Token>();

            while (tokens.Count > 0)
            {
                tok = tokens.Dequeue();
                if (tok.kind == startBracket)
                {
                    start++;
                    subTokens.Enqueue(tok);
                }
                else if (tok.kind == endBracket)
                {
                    end++;
                    if (end != start)
                        subTokens.Enqueue(tok);
                    else
                        break;
                }
                else if (tok.kind == seperator)
                {
                    exs.Add(Parse(subTokens));
                }
                else
                {
                    subTokens.Enqueue(tok);
                }
            }
            if (start != end)
            {
                exs.Clear();
                exs.Add(ErrorHandler("Missing end bracket"));
                return exs;
            }

            exs.Add(Parse(subTokens));

            return exs;
        }

        public Expression ParseNumber(Token tok, bool negative = false)
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

        public Expression ParseFunction(Token tok, Queue<Token> tokens)
        {
            Expression res;

            var args = ExtractBrackets (TokenKind.ParenthesesStart, tokens, TokenKind.Comma);

            switch (tok.value.ToLower())
            {
                case "sin":
                    res = new Sin(args, evaluator);
                    break;
                case "cos":
                    res = new Cos(args, evaluator);
                    break;
                case "tan":
                    res = new Tan(args, evaluator);
                    break;
                case "asin":
                    res = new ASin(args, evaluator);
                    break;
                case "acos":
                    res = new ACos(args, evaluator);
                    break;
                case "atan":
                    res = new ATan(args, evaluator);
                    break;
                case "sqrt":
                    res = new Sqrt(args, evaluator);
                    break;
                case "simplify":
                    res = new Simplify(args, evaluator);
                    break;
                case "expand":
                    res = new Expand(args, evaluator);
                    break;
                case "range":
                    res = new Range(args, evaluator);
                    break;
                case "plot":
                    res = new Plot(args, evaluator);
                    break;
                case "solve":
                    res = new Solve(args, evaluator);
                    break;
                default:
                    res = new UserDefinedFunction(tok.value.ToLower(), args, evaluator);
                    break;
            }

            return res;

        }
        public Error ErrorHandler(string message)
        {
            error = new Error(this, message);

            return error;
        }
    }
}

