﻿using System;

namespace Geomet
{
    public class Sphere
    {
        public static double Volume(double radius)
        {
            return 4 / 3 * Math.PI * radius * radius * radius;
        }
        public static double SurfaceArea(double radius)
        {
            return 4 * Math.PI * radius * radius;
        }
    }
}

