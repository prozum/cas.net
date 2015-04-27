using System;

namespace DesktopUI
{
    public class MovableDrawCanvas : MovableCasTextView
    {
        public DrawCanvas canvas;

        public MovableDrawCanvas()
            : base("", false)
        {
            canvas = new DrawCanvas();

            canvas.WidthRequest = 300;
            canvas.HeightRequest = 300;

            Remove(textview);
            textview = null;
            Attach(canvas, 1, 1, 1, 1);

            GLib.Timeout.Add(15, new GLib.TimeoutHandler(RedrawCanvas));

        }

        public bool RedrawCanvas()
        {
            canvas.QueueDraw();
            return true;
        }
    }
}

