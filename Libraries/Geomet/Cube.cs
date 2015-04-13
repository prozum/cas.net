using System;

namespace Geomet
{
    public class Cube
    {
        public static double volume(double length, double width, double height)
        { 
            return length * width * height;
        }
        public static double surfaceArea(double length, double width, double height)
        {
            return 2 * width * height + 2 * length * width + 2 * length * height;
        }
    }
}

