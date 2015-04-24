using System;
using Gtk;

namespace DesktopUI
{
    public class TeacherGetAssignmentListWindow : Window
    {
        User user;
        TextViewList textviews;

        public TeacherGetAssignmentListWindow(ref User user, ref TextViewList textviews)
            : base("Assignment List")
        {
            this.user = user;
            this.textviews = textviews;

        }
    }
}

