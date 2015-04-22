using System;
using Gtk;
using System.Net;

namespace DesktopUI
{
    public class LoginScreen : Window
    {
        int privilege;
        Menu menu;
        Entry entryUsername = new Entry();
        Entry entryPassword = new Entry();

        public LoginScreen(ref int privilege, Menu menu)
            : base("Login to CAS.NET")
        {
            this.privilege = privilege;
            this.menu = menu;

            SetDefaultSize(200, 200);

            VBox vbox = new VBox(false, 2);
            HBox hbox = new HBox(false, 2);

            Label labUsername = new Label("Username");
            Label labPassword = new Label("Password");

            Table table = new Table(2, 3, false);

            table.Attach(labUsername, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(labPassword, 0, 1, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            entryUsername.HeightRequest = 20;
            entryUsername.WidthRequest = 100;
            entryUsername.Buffer.Text = "";

            entryPassword.HeightRequest = 20;
            entryPassword.WidthRequest = 100;
            entryPassword.Buffer.Text = "";

            table.Attach(entryUsername, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(entryPassword, 1, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            Button buttonLogin = new Button("Login");

            buttonLogin.Clicked += delegate(object sender, EventArgs e)
            {
                ButtonLoginWrapper();
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

        void ButtonLoginWrapper()
        {
            string username = entryUsername.Text;
            string password = entryPassword.Text;

            const string host = "http://localhost:8080/";
            const string command = "Login";

            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            privilege = int.Parse(client.UploadString(host, command), System.Globalization.NumberStyles.AllowLeadingSign);

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

            Destroy();
        }
    }
}

