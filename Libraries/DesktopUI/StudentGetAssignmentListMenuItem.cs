using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    // Menu button for students to get assignment
    public class StudentGetAssignmentListMenuItem : MenuItem
    {
        TextViewList textviews;
        User user;

        // Constructor for studentgetassigmentlistmenuitem
        public StudentGetAssignmentListMenuItem(User user, TextViewList textviews)
            : base("Get List of Assignments")
        {
            this.textviews = textviews;
            this.user = user;
            Activated += (sender, e) => new StudentGetAssignmentListWindow(user, ref textviews);
        }
    }
}

