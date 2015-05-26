using System;
using Gtk;
using TaskGenLib;

namespace DesktopUI
{
    // Opens the taskgen menu for arithmetic tasks
    public class TaskGenAritMenuItem : MenuItem
    {
        TextViewList textviews;

        public TaskGenAritMenuItem(TextViewList textviews) : base("Arithmetic")
        {
            this.textviews = textviews;
            this.Activated += delegate
            {
                TaskGenAritWindow window = new TaskGenAritWindow(this.textviews);
            };
        }
    }
}
