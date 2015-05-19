using Gtk;
using Gdk;
using Ast;
using Cairo;
using System.Collections.Generic;

namespace Draw
{
    public class DrawView : DrawingArea
    {
        Surface surface = null;

        Pango.Layout layout;
        int h, w;
        double scale = 20;
        List<Real> xList;
        List<Real> yList;

        public DrawView()
        {
            //SetSizeRequest(600, 600); // Will be set by the program
            Drawn += new DrawnHandler(Redraw);
            ConfigureEvent += new ConfigureEventHandler(Configure);
        }

        public void Plot(PlotData data)
        {
            xList = data.x;
            yList = data.y;
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
            if (xList == null || yList == null)
                return;

            for(int i = 0; i < xList.Count; i++)
            {
                ct.LineTo(((double)xList[i].@decimal / scale + 0.5) * w, 
                    ((double)-yList[i].@decimal / scale + 0.5) * h);
            }
            ct.Stroke();
        }

        protected void Redraw(object o, DrawnArgs args)
        {
            var cr = args.Cr;

            cr.SetSourceSurface(surface, 0, 0);
            cr.Paint();
            var ct = args.Cr;

            w = AllocatedWidth;
            h = AllocatedHeight;
            layout = Pango.CairoHelper.CreateLayout(ct);
            ct.LineWidth = 1;

            DrawAxes(ct);
            DrawGrid(ct);
            DrawGraph(ct);
            args.RetVal = true;
        }

        protected void Configure(object o, ConfigureEventArgs args)
        {
            Widget widget = o as Widget;

            if (surface != null)
                surface.Dispose();

            var allocation = widget.Allocation;
            surface = widget.Window.CreateSimilarSurface(Cairo.Content.Color, allocation.Width, allocation.Height);

            var cr = new Context(surface);

            cr.SetSourceRGB(1, 1, 1);
            cr.Paint();
            cr.Dispose();

            args.RetVal = true;
        }

    }


}