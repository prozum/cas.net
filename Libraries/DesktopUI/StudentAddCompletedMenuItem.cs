using System;
using Gtk;
using ImEx;
using System.Collections.Generic;

namespace DesktopUI
{
    public class StudentAddCompletedMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public StudentAddCompletedMenuItem(ref User user, ref TextViewList textviews)
            : base("Add Completed")
        {
            this.user = user;
            this.textviews = textviews;

            this.Activated += delegate
            {
                OnClicked();
            };
        }

        void OnClicked()
        {
            StudentAddCompletedWindow window = new StudentAddCompletedWindow(ref user, ref textviews);
        }
    }
}

