using System;
using System.Collections.Generic;

namespace Ast
{
	public class Evaluator
	{
		public Dictionary<string,Expression> definitions = new Dictionary<string,Expression>();

		public Evaluator ()
		{
		}

        public Expression Evaluation(string inputString)
        {
            var exp = Parser.Parse(definitions, inputString);

            return exp.Evaluate();

            return new Error("Duuurh");
        }
	}
}

