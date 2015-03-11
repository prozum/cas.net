using Element;
using System.Collections.Generic;

namespace Expression
{
	class Expression : List<Element.Element>
	{
		public Expression()
		{
		}

		public static Expression operator +(Expression e1, Expression e2)
		{
			e1.AddRange (e2);

			return e1;
		}

		public static Expression operator -(Expression e1, Expression e2)
		{
			e1.Add (new Operator ("-"));
			e1.AddRange (e2);

			return e1;
		}
	}
}