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
//            throw new NotImplementedException();

            Window window = new Window("Get Assignment List");
            Console.WriteLine("Getting list of assignments for students");

            string[] assignmentList = user.student.GetAssignmentList();

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            VBox vbox = new VBox(false, 2);

            foreach (var item in assignmentList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Button button = new Button(item);
                    button.Clicked += delegate
                    {
                        Console.WriteLine(item);
                    };
                    vbox.Add(button);
                }
            }

            scrolledWindow.Add(vbox);
            window.Add(scrolledWindow);

            window.SetDefaultSize(300, 200);

            window.ShowAll();
        }
    }
}

