using System;

namespace Ast
{
	public class Equation : Expression
	{
		public Expression left;
		public Expression right;

		public override string ToString()
		{
			return left.ToString () + right.ToString ();
		}
	}
}

