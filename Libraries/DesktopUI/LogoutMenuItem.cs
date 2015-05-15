using System;
using Gtk;

namespace DesktopUI
{
    public class LogoutMenuItem : MenuItem
    {
        Menu menu;
        User user;

        // Constructor for the logout menu item
        public LogoutMenuItem(User user, Menu menu)
            : base("Logout")
        {
            this.menu = menu;
            this.user = user;

            this.Activated += delegate
            {
                LogoutWrapper();
            };
        }

        // Logs the user out when called, and hides all irrelevant menus.
        void LogoutWrapper()
        {
            user.Logout();

            foreach (Widget w in menu)
            {
                if (w.GetType() == typeof(StudentGetAssignmentListMenuItem)
                    || w.GetType() == typeof(TeacherAddAssignmentMenuItem)
                    || w.GetType() == typeof(TeacherGetAssignmentListMenuItem)
                    || w.GetType() == typeof(LogoutMenuItem))
                {
                    w.Hide();
                }
                else if (w.GetType() == typeof(LoginMenuItem))
                {
                    w.Show();
                }
            }
        }
    }
}

