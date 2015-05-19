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
        public decimal x1;
        public decimal y1;

        public decimal x2;
        public decimal y2;

        public LineData(decimal x1, decimal y1, decimal x2, decimal y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
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

//    public class PlotData : DrawData
//    {
//        public Expression exp;
//        public Variable sym;
//
//        public PlotData(PlotFunc plot)
//        {
//            this.sym = plot.@var;
//            this.exp = plot.expr;
//        }
//    }
}

