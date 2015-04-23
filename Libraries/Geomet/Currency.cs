using System;

namespace Geomet
{
    public class Currency
    {
        public static double danishToOther(double u_kurs, double money)
        {
            return (u_kurs/100)*money;
        }
		public static double otherToDanish(double u_kurs, double money)
        {
			return (money*u_kurs)/100;
        }
    }
}

