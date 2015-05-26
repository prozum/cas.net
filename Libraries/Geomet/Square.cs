using System;

namespace Geomet
{
    public class Square
    {
        public static double Area(double width, double length)
        {
            return length*width;
        }

        public static double Circumference(double width, double length)
        {
            return 2 * length + 2 * width;
        } 
    }
}

