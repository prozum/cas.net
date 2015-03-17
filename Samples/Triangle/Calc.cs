using System;

namespace Geometry
{
	public class Triangle
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
		// Trigonometri, find vinkler ud fra sidelængder
		public static double Trigono(char whichSide, double a, double b, double c) {
			if (whichSide == 'A') {
				if (a != 0 && c != 0) {
					return Math.Asin(Math.Sin (a / c));
				} else if (b != 0 && c != 0) {
					return Math.Cosh(Math.Cos (b / c));
				} else {
					return 0;
				}

			} else if (whichSide == 'B') {
				//hyp/kateta
				return 0;
			} else if (whichSide == 'C') {
				return 0;
			} else {
				return 0;
			}
		}
	}
}
