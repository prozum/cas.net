using System;

namespace Geomet
{
    public class Square
    {
        public static double Area(double width, double height)
        {
            return height*width;
        }

        public static double Circumference(double width, double height)
        {
            return 2 * height + 2 * width;
        } 
    }
}

