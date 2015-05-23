using System;

namespace Ast
{
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
}

