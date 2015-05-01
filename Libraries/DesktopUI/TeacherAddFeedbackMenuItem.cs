using System;
using Gtk;
using ImEx;
using System.Collections.Generic;

namespace DesktopUI
{
    public class TeacherAddFeedbackMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public TeacherAddFeedbackMenuItem(ref User user, ref TextViewList textviews)
            : base("Add Feedback")
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
            //TeacherAddFeedbackWindow window = new TeacherAddFeedbackWindow(ref user, ref textviews);
        }
    }
}

