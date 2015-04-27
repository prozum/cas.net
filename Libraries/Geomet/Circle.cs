using System;

namespace Geomet
{
    public class Circle
    {
        public static double Area(double radius)
        {
            return Math.PI * radius * radius;
        }

        public static double Circumference(double radius)
        {
            return 2*Math.PI * radius;
        }

    }
}

