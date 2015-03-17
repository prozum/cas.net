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

		    	// Eat ')'
                parseEnum.MoveNext();

            }
		    
			return substring;
		}

        private static Expression ParseParenthese (CharEnumerator parseEnum)
        {
			return Parse(ExtractSubstring (parseEnum));  
        }

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

		private static Expression ParseFunction(string identifier, CharEnumerator parseEnum)
		{
			var args = new List<Expression> ();

			var argString = ExtractSubstring (parseEnum);
			var argList = argString.Split (',');

			foreach (string arg in argList) {

				args.Add(Parse(arg));
			}
			return new Function(identifier, args);
		}

        enum NumberType { Integer, Rational, Irrational, Complex };

		public static Expression ParseNumber(CharEnumerator parseEnum)
		{
            NumberType resultType = NumberType.Integer;
            Expression result = new Expression();
			string number = "";

            while (true)
            {
                if (char.IsDigit(parseEnum.Current))
                {
                    number += parseEnum.Current;
                }
                else if (parseEnum.Current == '.')
                {
                    if (resultType == NumberType.Irrational)
                    {
                        result = null;
                        return result;
                    }

				    number += parseEnum.Current;
                    resultType = NumberType.Irrational;
                }
                else if (parseEnum.Current == 'i')
                {
                    resultType = NumberType.Complex;
                    break;
                }
            }

            switch (resultType)
            {
                case NumberType.Integer:
                    break;
                case NumberType.Irrational:
                    break;
                case NumberType.Complex:
                    break;
                default:
                    break;
            }

            return result;
		}
	}
}

