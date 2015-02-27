using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Cairo;

namespace Canvas
{
	public class DrawCanvas : DrawingArea
	{
		private List<Figure> Figures;
		private Mouse mouse;

		public DrawCanvas()
		{
			this.AddEvents((int)(EventMask.ButtonPressMask | EventMask.ButtonMotionMask));
			Figures = new List<Figure>();
			mouse = new Mouse();
			GLib.Timeout.Add (1, new GLib.TimeoutHandler (CoordAddDelay));
		}

		protected override bool OnDrawn (Context ctx)
		{
			ctx.LineWidth = 5;

			foreach (var fig in Figures) {
				fig.DrawFigure(ref ctx);
			}

			return true;
		}

		protected override bool OnButtonPressEvent (EventButton evnt)
		{
			mouse.UpdateCoord(evnt.X, evnt.Y);
			Figures.Add(new Figure(mouse.X, mouse.Y));
			mouse.Pressed = true;
			return true;
		}

		protected override bool OnButtonReleaseEvent (EventButton evnt)
		{
			mouse.Pressed = false;
			return true;
		}

		protected override bool OnMotionNotifyEvent (EventMotion evnt)
		{
			mouse.UpdateCoord(evnt.X, evnt.Y);
			return true;
		}

		public bool CoordAddDelay()
		{
			if (mouse.Pressed) {
				Figures[Figures.Count - 1].Add(mouse.X, mouse.Y);
			}

			return true;
		}
	}
}