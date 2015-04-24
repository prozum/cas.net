using System;
using Gtk;

namespace DesktopUI
{
    public class TeacherGetCompletedWindow : Window
    {
        User user;
        TextViewList textviews;

        public TeacherGetCompletedWindow(ref User user, ref TextViewList textviews)
            : base("Get Completed")
        {
            this.user = user;
            this.textviews = textviews;

            throw new NotImplementedException();
        }
    }
}

