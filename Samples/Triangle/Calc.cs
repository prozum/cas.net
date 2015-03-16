using System;

namespace Triangle
{
	public class Calc
	{
		public static double Pythagoras(double kat_1, double kat_2, double hyp)
		{
			if (kat_1 > 0 && kat_2 > 0) {
				return Math.Sqrt (Math.Pow (kat_1, 2) + Math.Pow (kat_2, 2));
			}
			else if ( hyp > 0) {
				if (kat_1>1){
					return Math.Sqrt (Math.Pow (hyp, 2) - Math.Pow (kat_1, 2));
				}
				else {
					return Math.Sqrt (Math.Pow (hyp, 2) - Math.Pow (kat_2, 2));
				}
			}
			else {
				return 0;
			}
		}
	}
}

