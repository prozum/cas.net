using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Ast
{
	public static class Parser
	{
	    static readonly char[] opValidChars = { '=', '<', '>', '+', '-', '*', '/', '^' };

        public static Expression Parse(Evaluator evaluator, string parseString)
        {
			Expression curExp;
			var exs = new Stack<Expression> ();
			var ops = new Stack<Operator> (); 

			var parseReader = new StringReader (parseString);

			int curChar;

			while ((curChar = parseReader.Peek()) != -1) {

                // Skip whitespace
				while (char.IsWhiteSpace ((char)curChar)) {

                    parseReader.Read();
                }

				if (char.IsLetter ((char)curChar)) {

					//exs.Push (ParseIdentifier (parseReader));
                    curExp = ParseIdentifier(evaluator, parseReader);
					exs.Push (curExp);

				} else if (char.IsDigit ((char)curChar)) {

					//exs.Push (ParseNumber (parseReader));
					curExp = ParseNumber (parseReader);
					exs.Push (curExp);

				} else if (curChar.Equals ('(')) {
				
					//exs.Push (Parse (ExtractSubstring (parseReader))); 
                    curExp = Parse(evaluator, ExtractSubstring(parseReader)); 
					exs.Push (curExp); 

				} else if (opValidChars.Contains ((char)curChar)) {

					//ops.Push (ParseOperator (parseReader));
					curExp = ParseOperator (parseReader);
					ops.Push ((Operator)curExp);

				} else {

					curExp = new Error ("Error in :" + parseReader.ToString());
				}

				if (curExp is Error) {

					return curExp;
				}
            }

			return CreateAst (exs, ops);
        }

		private static string ExtractSubstring(StringReader parseReader)
		{
			string substring = null;

			int parentEnd = 0;
			int parentStart = 0;

			if (((char)parseReader.Peek()).Equals('('))
		    {
                parseReader.Read();

				while (!((char)parseReader.Peek()).Equals(')') && (parentStart == parentEnd))
                {
					substring += (char)parseReader.Peek();
                    
					switch ((char)parseReader.Peek())
                    {
                        case '(':
                            parentStart++;
                            break;
                        case ')':
                            parentEnd++;
                            break;
                    }

                    parseReader.Read();
                }

				// Eat ')'
                parseReader.Read();

            }
		    
			return substring;
		}

		private static Expression CreateAst(Stack<Expression> exs, Stack<Operator> ops)
        {
			Expression left,right;
			Operator curOp = null, nextOp;

			if (exs.Count == 1) {

				return exs.Pop ();
			}

			right = exs.Pop ();

			while (ops.Count > 0 ) {

				curOp = ops.Pop ();
				curOp.right = right;
				right.parent = curOp;

				if (ops.Count > 0) {

					nextOp = ops.Peek ();

					if (curOp.priority > nextOp.priority) {

						curOp.left = exs.Pop ();

						if (curOp.parent == null) {

							curOp.parent = nextOp;
							right = curOp;

						} else {

							curOp.parent.parent = nextOp;
							right = curOp.parent;

						}

					} else {

						curOp.left = nextOp;
						nextOp.parent = curOp;
						right = exs.Pop ();
					}

				} else {

					left = exs.Pop ();
					left.parent = curOp;
					curOp.left = left;
				}

			}

			while (curOp.parent != null) {

				curOp = (Operator)curOp.parent;
			}

			return curOp;
        }

        private static Expression ParseIdentifier(Evaluator evaluator, StringReader parseReader)
        {
			string identifier = "";
			int curChar;

			while ((curChar = parseReader.Peek()) != -1 && char.IsLetterOrDigit ((char)curChar)) {

				//int test = curChar;
				identifier += (char)curChar;
				parseReader.Read ();
            }

			if ((char)curChar == '(') {

                return ParseFunction(evaluator, identifier, parseReader);

            } else {

                return new Symbol(evaluator, identifier);

            }
        }

        private static Expression ParseFunction(Evaluator evaluator, string identifier, StringReader parseReader)
		{
            Expression res;
			var args = new List<Expression> ();

			var argString = ExtractSubstring (parseReader);
			var argList = argString.Split (',');

			foreach (string arg in argList) {

                args.Add(Parse(evaluator, arg));
			}

            res = new Function(identifier, args);

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
                    return new Integer(int.Parse(number));
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
			case "=":
				return new Equal ();
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

