using System;

//using Formula;
using SymbolicMath;

namespace FormulaSample
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Expression exp  = Expression.Parse("x+y");
			Symbol x = new Symbol("x");
			Symbol y = new Symbol("y");

			var exp = x + y + 300 - 100;

			Console.WriteLine(exp.Repr);

		}
	}
}
