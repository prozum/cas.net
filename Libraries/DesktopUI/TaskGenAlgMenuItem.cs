using System;
using Gtk;
using TaskGenLib;

namespace DesktopUI
{
    public class TaskGenAlgMenuItem : MenuItem
    {
        TextViewList textviews;

        public TaskGenAlgMenuItem(TextViewList textviews) : base("Algebra")
        {
            this.textviews = textviews;
            this.Activated += delegate
            {
                TaskGenAlgWindow window = new TaskGenAlgWindow(this.textviews);
            };
        }
    }
}
