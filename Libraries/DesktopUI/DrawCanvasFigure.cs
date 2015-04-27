using System;
using System.Collections.Generic;
using Cairo;

namespace DesktopUI
{
    public class DrawCanvasFigure
    {
        private List<double> X;
        private List<double> Y;

        public DrawCanvasFigure(double x, double y)
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

            for (int i = 0; i < X.Count; i++)
            {
                ctx.LineTo(X[i], Y[i]);
            }

            ctx.Stroke();
        }

    }
}

