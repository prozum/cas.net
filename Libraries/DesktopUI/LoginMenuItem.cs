using System;
using Gtk;

namespace DesktopUI
{
    public class LoginMenuItem : MenuItem
    {
        User user;
        Menu menu;

        // Constructor for login menu
        public LoginMenuItem(User user, Menu menu)
            : base("Login")
        {
            this.user = user;
            this.menu = menu;

            this.Activated += delegate(object sender, EventArgs e)
            {
                LoginScreenWrapper();
            };
        }

        // Displays the login screen when called
        void LoginScreenWrapper()
        {
            LoginScreen screen = new LoginScreen(ref user, ref menu);
        }
    }
}

