using System;
using Gtk;
using Gdk;
using System.Collections.Generic;
using Cairo;

namespace DesktopUI
{
    // Not used in the final product
    // Cant be saved, and goes full on the CPU when used
    public class DrawCanvas : DrawingArea
    {
        private List<DrawCanvasFigure> Figures;
        private DrawCanvasMouse mouse;

        public DrawCanvas()
            : base()
        {
            this.AddEvents((int)(EventMask.ButtonPressMask | EventMask.ButtonMotionMask));
            Figures = new List<DrawCanvasFigure>();
            mouse = new DrawCanvasMouse();
            GLib.Timeout.Add(1, new GLib.TimeoutHandler(CoordAddDelay));
        }

        // Apply color to drawn area of canvas.
        protected override bool OnDrawn(Context ctx)
        {
            ctx.LineWidth = 5;

            foreach (var fig in Figures)
            {
                fig.DrawFigure(ref ctx);
            }

            return true;
        }

        // Run when the mouse is pressed
        protected override bool OnButtonPressEvent(EventButton evnt)
        {
            mouse.UpdateCoord(evnt.X, evnt.Y);
            Figures.Add(new DrawCanvasFigure(mouse.X, mouse.Y));
            mouse.Pressed = true;
            return true;
        }

        // Run when the mouse in released
        protected override bool OnButtonReleaseEvent(EventButton evnt)
        {
            mouse.Pressed = false;
            return true;
        }

        // Run when the mouse moved
        protected override bool OnMotionNotifyEvent(EventMotion evnt)
        {
            mouse.UpdateCoord(evnt.X, evnt.Y);
            return true;
        }

        // Adds mouse tosition to figures.
        public bool CoordAddDelay()
        {
            if (mouse.Pressed)
            {
                Figures[Figures.Count - 1].Add(mouse.X, mouse.Y);
            }

            return true;
        }

    }
}

