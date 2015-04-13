using System;

namespace Geomet
{
    public class Circle
    {
        public static double area(double radius)
        {
            return Math.PI * radius * radius;
        }

        public static double circumference(double radius)
        {
            return 2*Math.PI * radius;
        }

    }
}

