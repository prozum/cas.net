using System;
using Gtk;

namespace DesktopUI
{
    public class StudentGetAssignmentListMenuItem : MenuItem
    {
        User user;

        public StudentGetAssignmentListMenuItem(ref User user)
            : base("Get List of Assignments")
        {
            this.user = user;
            this.Activated += delegate
            {
                Onclicked();
            };
        }

        void Onclicked()
        {
            Window window = new Window("Get Assignment List");

            window.SetDefaultSize(300, 200);

            window.ShowAll();
        }
    }
}

