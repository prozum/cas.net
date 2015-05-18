using Gtk;
using Cairo;
using Ast;

namespace Graph
{
    public class GraphView : DrawingArea
    {
        Pango.Layout layout;
        int h, w;
        int iter = 1000;
        double scale = 20;
        double a, b, c;

        public GraphView(PlotData plotData)
        {
            //plotData.sym.evaluator.
            SetSizeRequest(600, 600);
            this.a = a;
            this.b = b;
            this.c = c;
        }

        private void DrawAxes(Context ct)
        {
            ct.MoveTo(w * 0.5, 0);
            ct.LineTo(w * 0.5, h);
            ct.MoveTo(0, h * 0.5);
            ct.LineTo(w, h * 0.5);
            ct.Stroke();
        }

        private void DrawGrid(Context ct)
        {
            double hi, wi;

            for (int i = 0; i <= scale; i++)
            {
                ct.SetSourceRGB(0.7, 0.7, 0.7);
                hi = h - h * i / scale;
                ct.MoveTo(0, hi);
                ct.LineTo(w, hi);

                wi = w * (i / scale);
                ct.MoveTo(wi, 0);
                ct.LineTo(wi, h);

                ct.Stroke();

                // Grid Numbers
                ct.SetSourceRGB(0, 0, 0);
                layout.SetText((i - scale / 2).ToString());
                ct.MoveTo(wi, h / 2);
                Pango.CairoHelper.ShowLayout(ct, layout);
                ct.MoveTo(w / 2, hi);
                Pango.CairoHelper.ShowLayout(ct, layout);
                ct.Stroke();
            }
        }

        private void DrawGraph(Context ct)
        {
            double x, y;

            // Move context to first iteration
            x = -0.5 * scale;
            y = a * x * x + b * x + c;
            ct.MoveTo((x / scale + 0.5) * w, (-y / scale + 0.5) * h);


            for (int i = 1; i <= iter; i++)
            {
                x = ((double)i / iter - 0.5) * scale;
                y = a * x * x + b * x + c;

                ct.LineTo((x / scale + 0.5) * w, (-y / scale + 0.5) * h);
            }
            ct.Stroke();
        }

        protected override bool OnDrawn(Context ct)
        {
            w = AllocatedWidth;
            h = AllocatedHeight;
            layout = Pango.CairoHelper.CreateLayout(ct);
            ct.LineWidth = 1;

            DrawAxes(ct);
            DrawGrid(ct);
            DrawGraph(ct);

            return true;
        }
    }


}