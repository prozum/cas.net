using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Ast
{
    public class Parser
    {
        static readonly char[] opValidChars = {'=', '<', '>', '+', '-', '*', '/', '^', ':'};
        static readonly string[] programDefinedFunctions = { "sin", "cos", "tan", "asin", "acos", "atan", "sqrt", "simplify", "expand", "range", "plot" };

        Evaluator eval;

        public Parser(Evaluator eval)
        {
            this.eval = eval;
        }

        public Expression Parse(string parseString)
        {
            return Parse(new Evaluator (), parseString);
        }

        public Expression Parse(Evaluator evaluator, string parseString)
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
                    curChar = parseReader.Read();
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
                            curExp = new Error(this, "Empty parenthesis");
                            break;
                        case 1:
                            curExp = parExp[0];
                            exs.Push(curExp);
                            break;
                        default:
                            curExp = new Error(this, "Invalid ',' in parenthesis");
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
                    curExp = new Error(this, "Error in: " + parseReader.ToString());
                }

                if (curExp is Error) 
                {
                    return curExp;
                }
            }

            return CreateAst (exs, ops);
        }

        enum BracketType { Parenthesis, Curly };

        private List<Expression> ExtractBrackets(Evaluator evaluator, StringReader parseReader, BracketType type)
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
                    exs.Add (new Error(this, "Invalid> BracketType"));
                    return exs;
            }

            if (((char)parseReader.Peek()).Equals(Start))
            {
                parseReader.Read();

                while (!((char)parseReader.Peek()).Equals(End) || (parentStart != parentEnd))
                {
                    curChar = (char)parseReader.Peek();
                    
                    if (curChar.Equals(Start))
                    {
                        substring += curChar;
                        parentStart++;
                    } 
                    else if (curChar.Equals(End))
                    {
                        substring += curChar;
                        parentEnd++;
                    }
                    else if (curChar.Equals(Sep))
                    {
                        exs.Add (Parse(evaluator, substring));
                        substring = "";
                    }
                    else if (curChar.Equals('\uffff'))
                    {
                        exs.Add (new Error(this, "No end char"));
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

            exs.Add (Parse(evaluator, substring));
            
            return exs;
        }

        private Expression CreateAst(Stack<Expression> exs, Stack<Operator> ops)
        {
            Expression left,right;
            Operator curOp = null, nextOp;

            if (exs.Count == 1) 
            {
                return exs.Pop ();
            }
            else if (exs.Count == 0)
            {
                return new Error(this, "No expressions found");
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

        private Expression ParseIdentifier(Evaluator evaluator, StringReader parseReader)
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

        private Expression ParseFunction(Evaluator evaluator, string identifier, StringReader parseReader)
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
                        case "range":
                            res = new Range(identifier.ToLower(), new Integer(0), args[0], new Integer(1));
                            break;
                        default:
                            res = new Error(this, identifier + " have the wrong number of arguments");
                            break;
                    }
                }
                else if (args.Count == 2)
                {
                    switch (identifier.ToLower())
                    {
                        case "plot":
                            if ((args[1] is Symbol) && args[0].ContainsNotNumber(args[1] as Symbol))
                            {
                                res = new Plot(identifier.ToLower(), args[0].Simplify(), args[1] as Symbol);
                            }
                            else
                            {
                                res = new Error(this, " Could not plot " + args[1].ToString() + " in " + args[0].ToString());
                            }
                            break;
                        default:
                            res = new Error(this, identifier + " have the wrong number of arguments");
                            break;
                    }
                }
                else if (args.Count == 3)
                {
                    switch (identifier.ToLower())
                    {
                        case "range":
                            res = new Range(identifier.ToLower(), args[0], args[1], args[2]);
                            break;
                        default:
                            res = new Error(this, identifier + " have the wrong number of arguments");
                            break;
                    }
                }
                else
                {
                    res = new Error(this, identifier + " have the wrong number of arguments");
                }
            }
            else
            {
                res = new UserDefinedFunction(identifier, args);
            }

            res.evaluator = evaluator;
            return res;
        }

        private Expression ParseList(Evaluator evaluator, StringReader parseReader)
        {
            Ast.List res;
            res = new Ast.List();

            res.elements = ExtractBrackets (evaluator, parseReader, BracketType.Curly);
            res.evaluator = evaluator;

            return res;
        }

        enum NumberType { Integer, Rational, Irrational, Complex };

        public Expression ParseNumber(StringReader parseReader)
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
                        return new Error(this, "unexpected extra decimal seperator in: " + parseReader.ToString());
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
                    Int64 intRes;
                    if (Int64.TryParse(number, out intRes))
                        return new Integer(intRes);
                    else
                        return new Error(this, "Integer overflow");
                case NumberType.Irrational:
                    decimal decRes;
                    if (decimal.TryParse(number, out decRes))
                        return new Irrational(decRes);
                    else
                        return new Error(this, "Decimal overflow");
                case NumberType.Complex:
                    return new Error(this, "Complex numbers not supported yet");
                    //return new Complex();
                default:
                    return new Error(this, "unknown error in:" + parseReader.ToString());
            }
        }


        enum OperatorType { Equal, LesserThan, GreaterThan, Plus, Minus, Mul, Div };

        private Expression ParseOperator(StringReader parseReader)
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
                return new Error(this, "operator not supported: " + op);
            }
        }
    }
}

