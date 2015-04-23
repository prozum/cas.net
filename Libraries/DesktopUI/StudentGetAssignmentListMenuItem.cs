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
            throw new NotImplementedException();

            Window window = new Window("Get Assignment List");

            string[] assignmentList = user.student.GetAssignmentList();

            if (assignmentList != null)
            {
                foreach (var item in assignmentList)
                {
                    Console.WriteLine(item);
                }
            }

            window.SetDefaultSize(300, 200);

            window.ShowAll();
        }
    }
}

