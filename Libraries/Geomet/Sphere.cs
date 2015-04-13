using System;

namespace Geometry
{
    public class Sphere
    {
        public static double volume(double radius)
        {
            return 4 / 3 * Math.PI * radius * radius * radius;
        }
        public static double surfaceArea(double radius)
        {
            return 4 * Math.PI * radius * radius;
        }
    }
}

