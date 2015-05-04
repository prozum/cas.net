using System;
using Gtk;
using TaskGenLib;

namespace DesktopUI
{
    public class TaskGenMenuItem : MenuItem
    {
        TextViewList textviews;

        public TaskGenMenuItem(TextViewList textviews) : base("Task Gen")
        {
            this.textviews = textviews;
            this.Activated += delegate
            {
                OnActivated();
            };
        }
   
        void OnActivated()
        {
            TaskGenWindow window = new TaskGenWindow(textviews);
        }
    }
}
