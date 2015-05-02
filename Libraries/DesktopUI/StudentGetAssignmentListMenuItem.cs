using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    public class StudentGetAssignmentListMenuItem : MenuItem
    {
        TextViewList textviews;
        User user;

        public StudentGetAssignmentListMenuItem(User user, ref TextViewList textviews)
            : base("Get List of Assignments")
        {
            this.textviews = textviews;
            this.user = user;
            this.Activated += delegate
            {
                Onclicked();
            };
        }

        void Onclicked()
        {
            StudentGetAssignmentListWindow window = new StudentGetAssignmentListWindow(user, ref textviews);
        }
    }
}

