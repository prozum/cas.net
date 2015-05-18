using System;

namespace Ast
{
    public abstract class DrawData : EvalData
    {
    }

    public class DotData : DrawData
    {
        public int x;
        public int y;
    }

    public class LineData : DrawData
    {
        public int x1;
        public int y1;

        public int x2;
        public int y2;
    }

    public class CircData : DrawData
    {
        public int centerX;
        public int centerY;

        public int radius;
    }

    public class TextData : DrawData
    {
        public string text;
    }

    public class PlotData : DrawData
    {
        public Expression exp;
        public Variable sym;

        public PlotData(PlotFunc plot)
        {
            this.sym = plot.sym;
            this.exp = plot.exp;
        }
    }
}

