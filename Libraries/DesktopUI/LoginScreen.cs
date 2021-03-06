﻿using System;
using Gtk;
using System.Net;

namespace DesktopUI
{
    // Login screen that allows the user to login to the server
    public class LoginScreen : Window
    {
        User user;
        Menu menu;
        Entry entryUsername = new Entry();
        Entry entryPassword = new Entry();
        Label errorLabel = new Label("Type your username and\npassword to log in");

        // Constructor for the login screen
        // Builds all elements of the window, and displays it to the user
        public LoginScreen(ref User user, ref Menu menu)
            : base("Login to CAS.NET")
        {
            this.user = user;
            this.menu = menu;

            SetDefaultSize(200, 200);

            VBox vbox = new VBox(false, 2);
            HBox hbox = new HBox(false, 2);

            Label labUsername = new Label("Username");
            Label labPassword = new Label("Password");

            Table table = new Table(2, 2, false);

            table.Attach(labUsername, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(labPassword, 0, 1, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            entryUsername.HeightRequest = 20;
            entryUsername.WidthRequest = 100;
            entryUsername.Buffer.Text = "";

            entryPassword.HeightRequest = 20;
            entryPassword.WidthRequest = 100;
            entryPassword.Buffer.Text = "";
            entryPassword.Visibility = false;

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
            vbox.Add(errorLabel);
            vbox.Add(hbox);

            Add(vbox);

            ShowAll();
        }

        // When the login button is pressed, it will connect to the server, and verify the user.
        void ButtonLoginWrapper()
        {
            string username = entryUsername.Text;
            string password = entryPassword.Text;

            const string host = "http://localhost:8080/";
            const string command = "Login";

            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            // Tries to connect to the server
            try
            {
                user.privilege = int.Parse(client.UploadString(host, command), System.Globalization.NumberStyles.AllowLeadingSign);

                if (user.privilege >= 0)
                {
                    user.Login(username, password, host);
                    user.ShowMenuItems(ref menu);

                    Destroy();
                }
                else
                {
                    errorLabel.Text = "Invalid user!\nPlease check your username\nand password, and try again";
                    errorLabel.Show();
                }
            }
            // Catches all problems with connecting to the server on login
            catch (Exception)
            {
                errorLabel.Text = "Server is down or not found!";
                errorLabel.Show();
            }
        }
    }
}

