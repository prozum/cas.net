using System;
using System.Collections.Generic;
using Cairo;

namespace Canvas
{
	public class Figure
	{
		private List<double> X;
		private List<double> Y;

		public Figure(double x, double y)
		{
			X = new List<double>();
			Y = new List<double>();

			X.Add(x);
			Y.Add(y);
		}

		public void Add(double x, double y)
		{
			X.Add(x);
			Y.Add(y);
		}

		public void DrawFigure(ref Context ctx)
		{
			ctx.MoveTo(X[0], Y[0]);
			ctx.Stroke();

			if (X.Count == 1) {
				ctx.Arc(X[0], Y[0], ctx.LineWidth/2, 0, 2*3.1415);
				ctx.Fill();
			} else {
				for (int i = 1; i < X.Count; i++) {
					ctx.LineTo(X[i], Y[i]);
				}

				ctx.Stroke();
			}
		}
	}
}

