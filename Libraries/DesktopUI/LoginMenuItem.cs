using System;
using Gtk;

namespace DesktopUI
{
	public class LoginMenuItem : MenuItem
	{
		public LoginMenuItem(ref int privilege, Menu menu) : base("Login")
		{
			this.Activated += delegate(object sender, EventArgs e)
				{
					LoginScreen screen = new LoginScreen(privilege, menu);
				};
		}
	}
}

