using System;
using System.Collections.Generic;

namespace Ast
{
	public static class Parser
	{
		private static Dictionary<string,int> predence = new Dictionary<string, int>
		{
			{"+",10},
			{"-",10},
			{"*",20},
			{"/",20}
		};

        public static Expression Parse(string parseString)
        {
            var exs = new List<Expression> ();
            var ops = new List<Operator> (); 

            var parseEnum = parseString.GetEnumerator ();


            while (parseEnum.MoveNext()) {

                // Skip whitespace
                while (char.IsWhiteSpace (parseEnum.Current) || parseEnum.Current.Equals(";")) {

                    parseEnum.MoveNext ();
                }

                if (char.IsLetter (parseEnum.Current)) {

                    exs.Add(ParseIdentifier (parseEnum));

                }

                if (char.IsDigit(parseEnum.Current)) {

                    exs.Add(ParseNumber (parseEnum));
				
                }

                if (parseEnum.Current.Equals("(")) {
				
                    exs.Add(ParseParenthese (parseEnum));
                }
            }

            //return CreateAst (exs, ops);
			return new Expression ();
        }

		public static string ExtractSubstring(CharEnumerator parseEnum)
		{
			string substring = null;

			int parentEnd = 0;
			int parentStart = 0;

			while (parseEnum.MoveNext()) {

				if (parseEnum.Current.Equals('('))
				{
                    parseEnum.MoveNext();

                    while (!parseEnum.Current.Equals(')') && (parentStart == parentEnd))
                    {
                        substring += parseEnum.Current;
                        
                        switch (parseEnum.Current)
                        {
                            case '(':
                                parentStart++;
                                break;
                            case ')':
                                parentEnd++;
                                break;
                        }

                        parseEnum.MoveNext();
                    }

					return substring;
                }
			}

            return substring;
		}

        private static Expression ParseParenthese (CharEnumerator parseEnum)
        {
			return Parse(ExtractSubstring (parseEnum));  
        }

        //private static Expression CreateAst(List<Expression> exs, List<Operator> ops)
        //{
        //    return exs [0];
        //}

        private static Expression ParseIdentifier(CharEnumerator parseEnum)
        {
            string identifier = "";

            while (char.IsLetterOrDigit (parseEnum.Current)) {

                identifier += parseEnum.Current;
                parseEnum.MoveNext ();
            }

            if (parseEnum.Current.Equals("(")) {

                return ParseFunction (identifier, parseEnum);

            } else {

                return new Symbol (identifier);

            }
        }

        enum NumberType { Integer, Rational, Irrational, Complex };

		public static Expression ParseNumber(CharEnumerator parseEnum)
		{
            NumberType resultType = NumberType.Integer;
            Expression result;
			string number = "";

            do
            {
                if (char.IsDigit(parseEnum.Current))
                {
                    number += parseEnum.Current;
                }
                else if (parseEnum.Current == '.')
                {
                    //More than one dot. Error!
                    if (resultType == NumberType.Irrational)
                    {
                        return null;
                    }

                    number += parseEnum.Current;
                    resultType = NumberType.Irrational;
                }
                else if (parseEnum.Current == 'i')
                {
                    resultType = NumberType.Complex;
                    break;
                }
                else
                {
                    break;
                }
            } while (parseEnum.MoveNext());

            switch (resultType)
            {
                case NumberType.Integer:
                    result = new Integer(int.Parse(number));
                    break;
                case NumberType.Irrational:
                    result = new Irrational(decimal.Parse(number));
                    break;
                case NumberType.Complex:
                    result = new Complex();
                    break;
                default:
                    //Should never happen
                    result = new Expression();
                    break;
            }

            return result;
		}
	}
}

