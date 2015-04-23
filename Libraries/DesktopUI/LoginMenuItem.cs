using System;
using Gtk;

namespace DesktopUI
{
    public class LoginMenuItem : MenuItem
    {
        User user;
        Menu menu;

        public LoginMenuItem(ref User user, Menu menu)
            : base("Login")
        {
            this.user = user;
            this.menu = menu;

            this.Activated += delegate(object sender, EventArgs e)
            {
                LoginScreenWrapper();
            };
        }

        void LoginScreenWrapper()
        {
            LoginScreen screen = new LoginScreen(ref user, menu);
        }
    }
}

