using System;
using Gtk;

namespace DesktopUI
{
    // Menu item for teachergetassignmentlist, that opens the window once clicked.
    public class TeacherGetAssignmentListMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public TeacherGetAssignmentListMenuItem(User user, TextViewList textviews)
            : base("Get List Of Assignments")
        {
            this.user = user;
            this.textviews = textviews;
            Activated += (sender, e) => new TeacherGetAssignmentListWindow(user, ref textviews);
        }
    }
}

