using System;

namespace DesktopUI
{
    public class MovableDrawCanvas : MovableCasTextView
    {
        public DrawCanvas canvas;

        // Constructor for movabledrawcanvs
        public MovableDrawCanvas()
            : base("", false)
        {
            canvas = new DrawCanvas();

            canvas.WidthRequest = 300;
            canvas.HeightRequest = 300;

            Remove(textview);
            Attach(canvas, 1, 1, 1, 1);

            GLib.Timeout.Add(15, new GLib.TimeoutHandler(RedrawCanvas));

        }

        // This method is called when the canvas is updated.
        public bool RedrawCanvas()
        {
            canvas.QueueDraw();
            return true;
        }
    }
}

