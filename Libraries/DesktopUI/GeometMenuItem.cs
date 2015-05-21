using System;
using Gtk;

namespace DesktopUI
{
    public class GeometMenuItem : MenuItem
    {
        TextViewList textviews;

        public GeometMenuItem(TextViewList textviews) : base("Geometry Tools")
        {
            this.textviews = textviews;
            this.Activated += delegate
            {
                GeometWindow window = new GeometWindow(this.textviews);
            };
        }
    }
}