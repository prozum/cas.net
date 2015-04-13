using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Ast
{
    public static class Parser
    {
        static readonly char[] opValidChars = {'=', '<', '>', '+', '-', '*', '/', '^', ':'};
        static readonly string[] programDefinedFunctions = { "sin", "cos", "tan", "asin", "acos", "atan", "sqrt", "simplify", "expand" };

        public static Expression Parse(string parseString)
        {
            return Parse(new Evaluator (), parseString);
        }

        public static Expression Parse(Evaluator evaluator, string parseString)
        {
            var exs = new Stack<Expression> ();
            var ops = new Stack<Operator> ();

            var parseReader = new StringReader (parseString);

            List<Expression> parExp;

            Expression curExp;
            int curChar;

            while ((curChar = parseReader.Peek()) != -1) 
            {
                // Skip whitespace
                while (char.IsWhiteSpace ((char)curChar)) 
                {
                    parseReader.Read();
                }

                // Functions & Variables
                if (char.IsLetter((char)curChar))
                {
                    curExp = ParseIdentifier(evaluator, parseReader);
                    exs.Push(curExp);
                }
                // Numbers
                else if (char.IsDigit((char)curChar))
                {
                    curExp = ParseNumber(parseReader);
                    exs.Push(curExp);

                }
                // Parenthesis
                else if (curChar.Equals('('))
                {
                    parExp = ExtractBrackets(evaluator, parseReader, BracketType.Parenthesis); 

                    switch (parExp.Count())
                    {
                        case 0:
                            curExp = new Error("Empty parenthesis");
                            break;
                        case 1:
                            curExp = parExp[0];
                            exs.Push(curExp);
                            break;
                        default:
                            curExp = new Error("Invalid ',' in parenthesis");
                            break;
                    }

                }
                // Lists
                else if (curChar.Equals('{'))
                {
                    curExp = ParseList(evaluator, parseReader);
                    exs.Push(curExp);
                }
                // Operators
                else if (opValidChars.Contains ((char)curChar)) 
                {
                    curExp = ParseOperator (parseReader);
                    if (curExp is Operator)
                    {
                        ops.Push ((Operator)curExp);
                    }
                }
                else 
                {
                    curExp = new Error ("Error in: " + parseReader.ToString());
                }

                if (curExp is Error) 
                {
                    return curExp;
                }
            }

            return CreateAst (exs, ops);
        }

        enum BracketType { Parenthesis, Curly };

        private static List<Expression> ExtractBrackets(Evaluator evaluator, StringReader parseReader, BracketType type)
        {
            List<Expression> exs = new List<Expression> ();

            char Start, End, Sep;

            char curChar;
            string substring = "";

            int parentEnd = 0;
            int parentStart = 0;

            switch (type)
            {
                case BracketType.Parenthesis:
                    Start = '(';
                    End = ')';
                    Sep = ',';
                    break;
                case BracketType.Curly:
                    Start = '{';
                    End = '}';
                    Sep = ',';
                    break;
                default:
                    exs.Add (new Error("Invalid BracketType"));
                    return exs;
            }

            if (((char)parseReader.Peek()).Equals(Start))
            {
                parseReader.Read();

                while (!((char)parseReader.Peek()).Equals(End) && (parentStart == parentEnd))
                {
                    curChar = (char)parseReader.Peek();

                    if (curChar.Equals(Start))
                    {
                        parentStart++;
                    } 
                    else if (curChar.Equals(End))
                    {
                        parentEnd++;
                    }
                    else if (curChar.Equals(Sep))
                    {
                        exs.Add (Parser.Parse(evaluator, substring));
                        substring = "";
                    }
                    else if (curChar.Equals('\uffff'))
                    {
                        exs.Add (new Error("No end char"));
                        return exs;
                    }
                    else
                    {
                        substring += curChar;
                    }

                    parseReader.Read();
                }
                parseReader.Read();
            }

            exs.Add (Parser.Parse(evaluator, substring));
            
            return exs;
        }

        private static Expression CreateAst(Stack<Expression> exs, Stack<Operator> ops)
        {
            Expression left,right;
            Operator curOp = null, nextOp;

            if (exs.Count == 1) 
            {
                return exs.Pop ();
            }
            else if (exs.Count == 0)
            {
                return new Error("No expressions found");
            }

            right = exs.Pop ();

            while (ops.Count > 0 ) 
            {
                curOp = ops.Pop ();
                curOp.right = right;
                right.parent = curOp;

                if (ops.Count > 0) 
                {
                    nextOp = ops.Peek ();

                    if (curOp.priority >= nextOp.priority) 
                    {
                        curOp.left = exs.Pop ();

                        if (curOp.parent == null) 
                        {
                            curOp.parent = nextOp;
                            right = curOp;
                        } 
                        else 
                        {
                            curOp.parent.parent = nextOp;
                            right = curOp.parent;
                        }
                    } 
                    else
                    {
                        curOp.left = nextOp;
                        nextOp.parent = curOp;
                        right = exs.Pop ();
                    }
                } 
                else 
                {
                    left = exs.Pop ();
                    left.parent = curOp;
                    curOp.left = left;
                }

            }

            while (curOp.parent != null) 
            {
                curOp = (Operator)curOp.parent;
            }

            return curOp;
        }

        private static Expression ParseIdentifier(Evaluator evaluator, StringReader parseReader)
        {
            string identifier = "";
            int curChar;

            while ((curChar = parseReader.Peek()) != -1 && char.IsLetterOrDigit ((char)curChar))
            {
                identifier += (char)curChar;
                parseReader.Read ();
            }

            if ((char)curChar == '(')
            {

                return ParseFunction(evaluator, identifier, parseReader);

            } 
            else 
            {
                return new Symbol(evaluator, identifier);
            }
        }

        private static Expression ParseFunction(Evaluator evaluator, string identifier, StringReader parseReader)
        {
            Expression res;
            var args = ExtractBrackets (evaluator, parseReader, BracketType.Parenthesis);

            if (programDefinedFunctions.Contains(identifier.ToLower()))
            {
                if (args.Count == 1)
                {
                    switch (identifier.ToLower())
                    {
                    case "sin":
                        res = new Sin(identifier.ToLower(), args[0]);
                        break;
                    case "cos":
                        res = new Cos(identifier.ToLower(), args[0]);
                        break;
                    case "tan":
                        res = new Tan(identifier.ToLower(), args[0]);
                        break;
                    case "asin":
                        res = new ASin(identifier.ToLower(), args[0]);
                        break;
                    case "acos":
                        res = new ACos(identifier.ToLower(), args[0]);
                        break;
                    case "atan":
                        res = new ATan(identifier.ToLower(), args[0]);
                        break;
                    case "sqrt":
                        res = new Sqrt(identifier.ToLower(), args[0]);
                        break;
                    case "simplify":
                        res = new Simplify(identifier.ToLower(), args[0]);
                        break;
                    case "expand":
                        res = new Expand(identifier.ToLower(), args[0]);
                        break;
                    default:
                        res = new Error("This should never happen");
                        break;
                    }
                }
                else
                {
                    res = new Error("Unary operation can't have more than one argument");
                }
            }
            else
            {
                res = new UserDefinedFunction(identifier, args);
            }

            res.evaluator = evaluator;
            return res;
        }

        private static Expression ParseList(Evaluator evaluator, StringReader parseReader)
        {
            Ast.List res;
            res = new Ast.List();

            res.elements = ExtractBrackets (evaluator, parseReader, BracketType.Curly);
            res.evaluator = evaluator;

            return res;
        }

        enum NumberType { Integer, Rational, Irrational, Complex };

        public static Expression ParseNumber(StringReader parseReader)
        {
            NumberType resultType = NumberType.Integer;
            string number = "";

            do
            {
                if (char.IsDigit((char)parseReader.Peek()))
                {
                    number += (char)parseReader.Read();
                }
                else if ((char)parseReader.Peek() == NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator[0])
                {
                    //More than one dot. Error!
                    if (resultType == NumberType.Irrational )
                    {
                        return new Error("Parser: unexpected extra decimal seperator in: " + parseReader.ToString());
                    }

                    number += (char)parseReader.Read();
                    resultType = NumberType.Irrational;
                }
                else if ((char)parseReader.Peek() == 'i')
                {
                    resultType = NumberType.Complex;
                    break;
                }
                else
                {
                    break;
                }
            } while (parseReader.Peek() != -1);

            switch (resultType)
            {
                case NumberType.Integer:
                    return new Integer(Int64.Parse(number));
                case NumberType.Irrational:
                    return new Irrational(decimal.Parse(number));
                case NumberType.Complex:
                    return new Complex();
                default:
                    return new Error ("Parser: unknown error in:" + parseReader.ToString ());
            }
        }


        enum OperatorType { Equal, LesserThan, GreaterThan, Plus, Minus, Mul, Div };

        private static Expression ParseOperator(StringReader parseReader)
        {
            string op = ((char)parseReader.Read()).ToString();

            if (opValidChars.Contains((char)parseReader.Peek())) {

                op += (char)parseReader.Read();
            }
                
            switch (op)
            {
            case ":=":
                return new Assign();
            case "=":
//                return new Assign();
                return new Equal ();
            case "==":
                return new BooleanEqual();
            case "<=":
                return new LesserOrEqual();
            case ">=":
                return new GreaterOrEqual();
            case "<":
                return new Lesser ();
            case ">":
                return new Greater ();
            case "+":
                return new Add ();
            case "-":
                return new Sub ();
            case "*":
                return new Mul ();
            case "/":
                return new Div ();
            case "^":
                return new Exp ();
            default:
                return new Error ("Parser: operator not supported: " + op);
            }
        }
    }
}

