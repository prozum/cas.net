using System;

namespace Geomet
{
    public class Currency
    {
        public static double danishToOther(double u_kurs, double money)
        {
            return (u_kurs/100)*money;
        }
        public static double otherToDanish(double radius)
        {
            return 4 / 3 * Math.PI * radius * radius * radius;
        }
    }
}

