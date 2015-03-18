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
			Console.WriteLine ("Areal {0}", Triangle.Area (2, 2, 0, 2, 2, 90));
			//Console.WriteLine ("Angles {0}, {1}, {2}.", Triangle.Area (2, 3,5));
		}
	}
}
