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
				} else {
					return Math.Sqrt (Math.Pow (hyp, 2) - Math.Pow (kat_2, 2));
				}
			} else {
				return 0;
			}
		}
		public static double Area(double a, double b, double c, double A, double B, double C)
		{
			if (Verify (a, b, c)) {
				if (a != 0 && b != 0 && C != 0) {
					return 0.5 * a * b * Math.Sin (C);
				} else if (b != 0 && c != 0 && A != 0) {
					return 0.5 * b * c * Math.Sin (A);
				} else if (a != 0 && c != 0 && B != 0) {
					return 0.5 * a * c * Math.Sin (B);
				} else {
					return 0;
				}
			} else {
				return 0;
			}
		}
		//Check if valid triangle
		public static bool Verify(double a, double b, double c) {
			if (a + b > c || a + c > b || b + c > a) {
				return true;
			} else {
				return false;
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
