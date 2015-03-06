using System;

//using Formula;
using SymbolicMath;

namespace FormulaTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Expression exp  = Expression.Parse("x+y");
			Symbol x = new Symbol("x");
			Symbol y = new Symbol("y");

			var exp = x + y + 300/(x-100);

			Console.WriteLine(exp.Repr);

		}
	}
}
