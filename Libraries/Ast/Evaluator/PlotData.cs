using System.Collections.Generic;

namespace Ast
{
    public class PlotData : DrawData
    {
        public List<Real> x;
        public List<Real> y;
        public List<Real> z;

        public PlotData(List<Real> x, List<Real> y, List<Real> z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}

