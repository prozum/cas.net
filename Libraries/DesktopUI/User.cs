using System;
using Account;
using Gtk;

namespace DesktopUI
{
    public class User
    {
        public int privilege = -1; // <- standard privilege is -1, saying that the user is not logged in.
        public string username;

        public Student student;
        public Teacher teacher;

        // Empty constructor for the user, as the user is made before any information is known about the user.
        public User()
        {
        }

        // Sets up the user information on login, and creates a new instance of teacher and student, used for server-client communication
        public void Login(string username, string password, string host)
        {
            this.username = username;
            student = new Student(username, password, host);
            teacher = new Teacher(username, password, host);
        }

        // Resets the user
        public void Logout()
        {
            privilege = -1;
            student = null;
            teacher = null;
        }

        // Sets what menus the user can see while logged in.
        public void ShowMenuItems(ref Menu menu)
        {
            foreach (Widget w in menu)
            {
                // Shows the logout menu if a user of privilege 0 or 1 is logged in.
                if (privilege == 1 || privilege == 0)
                {
                    if (w.GetType() == typeof(LoginMenuItem))
                    {
                        w.Hide();
                    }
                    if (w.GetType() == typeof(LogoutMenuItem))
                        w.Show();
                }

                // Shows user specific menus based on privilege
                if (privilege == 0)
                {
                    if (w.GetType() == typeof(StudentGetAssignmentListMenuItem))
                    {
                        w.Show();
                    }
                }
                else if (privilege == 1)
                {
                    if (w.GetType() == typeof(TeacherAddAssignmentMenuItem))
                    {
                        w.Show();
                    }
                    else if (w.GetType() == typeof(TeacherGetAssignmentListMenuItem))
                    {
                        w.Show();
                    }
                }
            }
        }
    }
}

