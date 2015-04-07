using System;

namespace Canvas
{
    public class Mouse
    {
        /* Contains input device's x- and y-coordinate */
        public double X;
        public double Y;

        /* True if mouse button is pressed, false if not */
        public bool Pressed;

        public Mouse () 
        { 
            Pressed = false;
        }

        public void UpdateCoord(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}

