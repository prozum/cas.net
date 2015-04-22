using System;
using Gtk;

namespace DesktopUI
{
    public class LoginMenuItem : MenuItem
    {
        int privilege;
        Menu menu;

        public LoginMenuItem(ref int privilege, Menu menu)
            : base("Login")
        {
            this.privilege = privilege;
            this.menu = menu;

            this.Activated += delegate(object sender, EventArgs e)
            {
                LoginScreenWrapper();
            };
        }

        void LoginScreenWrapper()
        {
            LoginScreen screen = new LoginScreen(ref privilege, menu);
        }
    }
}

