using System;

namespace Geometry
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Længde beregning vha. pythagoras
			Console.WriteLine ("Længde af hypotenuse: {0}", Triangle.Pythagoras (2, 2, 0));
			Console.WriteLine ("A^b_C");

			Console.WriteLine ("Vinkel A: {0}", Triangle.Trigono ('A', 0, 3, 5));


		}
	}
}
