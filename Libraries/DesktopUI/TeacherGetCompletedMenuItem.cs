using System;
using Gtk;

namespace DesktopUI
{
    public class TeacherGetCompletedMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public TeacherGetCompletedMenuItem(ref User user, ref TextViewList textviews)
            : base("Get Completed")
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
            throw new NotImplementedException();
            // WHAT TO DO?
        }
    }
}

