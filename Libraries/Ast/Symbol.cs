using System;
using System.Collections.Generic;

namespace Ast
{
	public class Symbol : Expression
	{
		public Number prefix, exponent;
		public string symbol;

        public Symbol(Evaluator evaluator, string sym)
		{
			symbol = sym;
            this.evaluator = evaluator;
		}

		public override string ToString()
		{
			return symbol;
		}

		public override Expression Evaluate()
		{
            Expression res;

            if (this.functionCall is Function)
            {
                if (functionCall.tempDefinitions.ContainsKey(symbol))
                {
                    functionCall.tempDefinitions.TryGetValue(symbol, out res);
                    return res.Evaluate();
                }
            }
            else
            {
                if (evaluator.variableDefinitions.ContainsKey(symbol))
                {
                    evaluator.variableDefinitions.TryGetValue(symbol, out res);
                    return res.Evaluate();
                }
            }


            return new Error("Duuurh");
		}
	}
}

