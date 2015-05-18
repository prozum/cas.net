using System;
using Gtk;
using TaskGenLib;

namespace DesktopUI
{
    // Menu item for taskgen
    public class TaskGenMenuItem : MenuItem
    {
        TextViewList textviews;

        public TaskGenMenuItem(TextViewList textviews) : base("Task Gen")
        {

        }
    }
}
