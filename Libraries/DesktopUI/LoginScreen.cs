using System;
using Gtk;
using System.Net;

namespace DesktopUI
{
	public class LoginScreen : Window
	{
		public LoginScreen(ref int privilege, Menu menu) : base("Login to CAS.NET")
		{
			SetDefaultSize(200, 200);

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
					string username = entryUsername.Text;
					string password = entryPassword.Text;

					const string host = "http://localhost:8080/";
					const string command = "Login";

					var client = new WebClient();
					client.Encoding = System.Text.Encoding.UTF8;
					client.Credentials = new NetworkCredential(username, password);

					privilege = int.Parse(client.UploadString(host, command), System.Globalization.NumberStyles.AllowLeadingSign);

					Console.WriteLine("Your privilgee is: " + privilege);

					if (privilege == 0)
					{
						MenuItem menuItemStudentAddCompleted = new MenuItem("Upload Completed Task");

						MenuItem menuItemStudentGetAssignment = new MenuItem("Get Assignment");

						MenuItem menuItemStudentGetAssignmentList = new MenuItem("Get List Of Assignments");

						MenuItem menuItemStudentGetFeedback = new MenuItem("Get Feedback");


						menu.Append(menuItemStudentAddCompleted);
					}
					else if (privilege == 1)
					{
						MenuItem menuItemTeacherAddAssignment = new MenuItem("Add New Assignment");

						MenuItem menuItemTeacherAddFeedback = new MenuItem("Add Feedback");

						MenuItem menuItemTeacherGetAssignmentList = new MenuItem("Get List Of Assignments");

						MenuItem menuItemTeacherGetCompleted = new MenuItem("Get Completed Assignments");
					}

					menu.QueueDraw();
					Destroy();

				};



			Button buttonCancel = new Button("Cancel");
			buttonCancel.Clicked += (object sender, EventArgs e) => Destroy();

			hbox.Add(buttonCancel);
			hbox.Add(buttonLogin);

			vbox.Add(table);
			vbox.Add(hbox);

			Add(vbox);
			ShowAll();
		}
	}
}

