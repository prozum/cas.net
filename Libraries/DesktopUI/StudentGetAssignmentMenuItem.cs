using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    public class StudentGetAssignmentMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public StudentGetAssignmentMenuItem(ref User user, ref TextViewList textviews)
            : base("Get Assignment")
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
            StudentGetAssignmentWindow window = new StudentGetAssignmentWindow(ref user, ref textviews);
        }
    }
}

