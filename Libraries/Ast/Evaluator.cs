using System;
using System.Collections.Generic;

namespace Ast
{
	public class Evaluator
	{
        public Dictionary<string, Expression> variableDefinitions = new Dictionary<string, Expression>();
        public Dictionary<string, Expression> functionDefinitions = new Dictionary<string, Expression>();
        public Dictionary<string, List<string>> functionParams = new Dictionary<string, List<string>>();

		public Evaluator ()
		{
		}

        public Expression Evaluation(string inputString)
        {
            var exp = Parser.Parse(this, inputString);

            if (exp is Function)
            {
                (exp as Function).functionParams = functionParams;
                return exp.Evaluate();
            }
            else
            {
                return exp.Evaluate();
            }

            return new Error("Duuurh");
        }
	}
}

