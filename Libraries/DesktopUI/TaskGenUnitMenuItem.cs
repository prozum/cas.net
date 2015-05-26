using System;
using Gtk;
using TaskGenLib;

namespace DesktopUI
{
    // Task gen menu for unit tasks
    public class TaskGenUnitMenuItem : MenuItem
    {
        TextViewList textviews;

        public TaskGenUnitMenuItem(TextViewList textviews) : base("Unit Conversion")
        {
                this.textviews = textviews;
                this.Activated += delegate
                {
                    TaskGenUnitWindow window = new TaskGenUnitWindow(this.textviews);
                };
        }
    }
}

