using System;
using Gtk;

namespace DesktopUI
{
    // Menu item for accessing the Geometry tools
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