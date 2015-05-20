using System;

namespace Geomet
{
    public class Currency
    {
        public static double DanishToOther(double u_kurs, double money)
        {
            return (u_kurs/100)*money;
        }
		public static double OtherToDanish(double u_kurs, double money)
        {
			return (money*u_kurs)/100;
        }
    }
}