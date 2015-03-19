 using System;

namespace Geometry
{
	public class Triangle
	{
		//[RETVINKLET] Har to sider, find sidste side
		public static double Pythagoras(double kat_1, double kat_2, double hyp)
		{
			if (Verify (kat_1, kat_2, hyp)) {
				if (kat_1 > 0 && kat_2 > 0) {
					return Math.Sqrt (Math.Pow (kat_1, 2) + Math.Pow (kat_2, 2));
				} else if (hyp > 0) {
					if (kat_1 > 1) {
						return Math.Sqrt (Math.Pow (hyp, 2) - Math.Pow (kat_1, 2));
					} else {
						return Math.Sqrt (Math.Pow (hyp, 2) - Math.Pow (kat_2, 2));
					}
				} else {
					return 0;
				}
			} else {
				return 0;
			}
		}
		// [RETVINKLET] Trigonometri, find alle vinkler ud fra alle sidelængder
		public static double[] Angles(double a, double b, double c) {
			if (Verify(a,b,c)) {
				double[] angles = new double[3];
				angles[0] = (180/Math.PI)*Math.Acos(( Math.Pow(b,2) + Math.Pow(c,2) - Math.Pow(a,2) ) / ( 2*b*c ));
				angles[1] = (180/Math.PI)*Math.Acos(( Math.Pow(a,2) + Math.Pow(c,2) - Math.Pow(b,2) ) / ( 2*a*c ));
				angles[2] = (180/Math.PI)*Math.Acos(( Math.Pow(a,2) + Math.Pow(b,2) - Math.Pow(c,2) ) / ( 2*a*b ));
				return angles;
			}
			else {
				return null;
			}
		}
		//Have alle sider, find areal
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
			
	}
}
