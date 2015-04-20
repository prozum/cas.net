using System;
using Gtk;
using System.Net;

namespace DesktopUI
{
	public class ServerMenuItem : MenuItem
	{
		int privilege;
		string username;
		string password;

		public ServerMenuItem (ref int privilege) : base("Server")
		{
			this.privilege = privilege;

			Menu ServerMenu = new Menu ();
			this.Submenu = ServerMenu;

			MenuItem loginItem = new MenuItem("Login");
			loginItem.Activated += (object sender, EventArgs e) => LoginScreen();

			MenuItem logoutItem = new MenuItem("Logout");
			logoutItem.Activated += delegate(object sender, EventArgs e)
			{
				username = null;
				password = null;
			};

			ServerMenu.Append (loginItem);
			ServerMenu.Append (logoutItem);
		}

		void LoginScreen()
		{
			Window loginWindow = new Window("Login to CAS.NET");
			loginWindow.SetDefaultSize(200, 200);

			VBox vbox = new VBox(false, 2);
			HBox hbox = new HBox(false, 2);

			Label labUsername = new Label("Username");
			Label labPassword = new Label("Password");

			Table table = new Table(2, 3, false);

			table.Attach(labUsername, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
			table.Attach(labPassword, 0, 1, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

			Entry entryUsername = new Entry();
			entryUsername.HeightRequest = 20;
			entryUsername.WidthRequest = 100;
			entryUsername.Buffer.Text = "";

			Entry entryPassword = new Entry();
			entryPassword.HeightRequest = 20;
			entryPassword.WidthRequest = 100;
			entryPassword.Buffer.Text = "";

			table.Attach(entryUsername, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
			table.Attach(entryPassword, 1, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

			Button buttonLogin = new Button("Login");

			buttonLogin.Clicked += delegate(object sender, EventArgs e)
			{
				username = entryUsername.Text;
				password = entryPassword.Text;

				const string host = "http://localhost:8080/";
				const string command = "Login";

				var client = new WebClient();
				client.Encoding = System.Text.Encoding.UTF8;
				client.Credentials = new NetworkCredential(username, password);

				privilege = int.Parse(client.UploadString(host, command), System.Globalization.NumberStyles.AllowLeadingSign);

				loginWindow.Destroy();

			};



			Button buttonCancel = new Button("Cancel");
			buttonCancel.Clicked += (object sender, EventArgs e) => loginWindow.Destroy();

			hbox.Add(buttonCancel);
			hbox.Add(buttonLogin);

			vbox.Add(table);
			vbox.Add(hbox);

			loginWindow.Add(vbox);
			loginWindow.ShowAll();
		}
	}
}

