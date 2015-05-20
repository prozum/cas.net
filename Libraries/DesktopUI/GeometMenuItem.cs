using System;
using Gtk;

namespace DesktopUI
{
    public class GeometMenuItem : MenuItem
    {
        TextViewList textviews;

        public GeometMenuItem(TextViewList textviews) : base("Solver Tools")
        {
            this.textviews = textviews;
            this.Activated += delegate
            {
                GeometWindow window = new GeometWindow(this.textviews);
            };
        }
    }
}