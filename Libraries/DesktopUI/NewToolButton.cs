using System;
using Gtk;

namespace DesktopUI
{
    public class NewToolButton : ToolButton
    {
        public NewToolButton(TextViewList textviews)
            : base(Stock.New)
        {
            this.Clicked += delegate
            {
                textviews.castextviews.Clear();
                textviews.Clear();
                textviews.Redraw();
                textviews.ShowAll();
            };
        }
    }
}

