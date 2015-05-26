using System;

namespace DesktopUI
{
    // Used by drawcanvas
    public class DrawCanvasMouse
    {
        /* Contains input device's x- and y-coordinate */
        public double X;
        public double Y;

        /* True if mouse button is pressed, false if not */
        public bool Pressed;

        // Constructor for drawcanvasmouse
        public DrawCanvasMouse()
        { 
            Pressed = false;
        }

        // Updates the mouse coordinates
        public void UpdateCoord(double x, double y)
        {
            X = x;
            Y = y;
        }

    }
}

