using System;

namespace Ast
{
	public class Symbol : Expression
	{
		public Number prefix, exponent;
		public string symbol;

		public Symbol (string sym)
		{
			symbol = sym;
		}

		public override string ToString()
		{
			return symbol;
		}

		public override Expression Evaluate()
		{
			return this;
		}
	}
}

