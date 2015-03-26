using System;
using System.Collections.Generic;

namespace Ast
{
	public class Symbol : Expression
	{
		public Number prefix, exponent;
		public string symbol;

        public Symbol(Dictionary<string, Expression> definitions, string sym)
		{
			symbol = sym;
            this.definitions = definitions;
		}

		public override string ToString()
		{
			return symbol;
		}

		public override Expression Evaluate()
		{
            Expression res;

            if (definitions.ContainsKey(symbol))
            {
                definitions.TryGetValue(symbol, out res);
                return res.Evaluate();
            }

            return new Error("Duuurh");
		}
	}
}

