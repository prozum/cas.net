using System;
using Account;
using Gtk;

namespace DesktopUI
{
    public class User
    {
        public int privilege;
        public string username;

        public Student student;
        public Teacher teacher;

        public User()
        {
        }

        public void Login(string username, string password, string host)
        {
            this.username = username;
            student = new Student(username, password, host);
            teacher = new Teacher(username, password, host);
        }

        public void Logout()
        {
            privilege = -1;
            student = null;
            teacher = null;
        }

        public void ShowMenuItems(ref Menu menu)
        {
            foreach (Widget w in menu)
            {
                Console.WriteLine(w);

                if (privilege == 1 || privilege == 0)
                {
                    if (w.GetType() == typeof(LoginMenuItem))
                    {
                        w.Hide();
                    }
                    if (w.GetType() == typeof(LogoutMenuItem))
                        w.Show();
                }

                if (privilege == 0)
                {
                    if (w.GetType() == typeof(StudentGetAssignmentMenuItem))
                    {
                        w.Show();
                    }
                    else if (w.GetType() == typeof(StudentGetAssignmentListMenuItem))
                    {
                        w.Show();
                    }
                    else if (w.GetType() == typeof(StudentGetFeedbackMenuItem))
                    {
                        w.Show();
                    }
                    else if (w.GetType() == typeof(StudentAddCompletedMenuItem))
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
                    else if (w.GetType() == typeof(TeacherAddFeedbackMenuItem))
                    {
                        w.Show();
                    }
                    else if (w.GetType() == typeof(TeacherGetAssignmentListMenuItem))
                    {
                        w.Show();
                    }
                    else if (w.GetType() == typeof(TeacherGetCompletedMenuItem))
                    {
                        w.Show();
                    }
                }
            }
        }
    }
}

