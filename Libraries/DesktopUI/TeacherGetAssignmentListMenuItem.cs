using System;
using Gtk;

namespace DesktopUI
{
    public class TeacherGetAssignmentListMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public TeacherGetAssignmentListMenuItem(User user, ref TextViewList textviews)
            : base("Get List Of Assignments")
        {
            this.user = user;
            this.textviews = textviews;
            Activated += (sender, e) => new TeacherGetAssignmentListWindow(user, ref textviews);
        }
    }
}

