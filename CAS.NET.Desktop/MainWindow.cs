using System;
using Gtk;
using TaskGenLib;
using System.Collections.Generic;
using ImEx;
using System.Net;

namespace Desktop
{
    class MainWindow : Window
    {
        List<Widget> listWidget = new List<Widget>();
        List<MetaType> mt = new List<MetaType>();
        Grid globalGrid = new Grid();
        int gridNumber = 1;
        VBox vboxWindow = new VBox(false, 2);
        string casFile = "";
        string username, password;

        delegate int login(string username,string password);

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            new MainWindow();
            Application.Run();
        }

        public MainWindow()
            : base("CAS.NET")
        {
            DeleteEvent += (o, a) => Application.Quit();

            SetSizeRequest(300, 500);

            #region Menu

            MenuBar menuBar = new MenuBar();
            Menu fileMenu = new Menu();
            Menu addMenu = new Menu();
            Menu serverMenu = new Menu();


            MenuItem file = new MenuItem("File");
            file.Submenu = fileMenu;

            MenuItem newFile = new MenuItem("New File");
            newFile.Activated += (o, a) => ClearWindow();

            MenuItem openFile = new MenuItem("Open File");
            openFile.Activated += (o, a) => OpenFile();

            MenuItem saveFile = new MenuItem("Save File");
            saveFile.Activated += (o, a) => SaveFile();



            MenuItem addItem = new MenuItem("Add");
            addItem.Submenu = addMenu;

            MenuItem addEntry = new MenuItem("Entry");
            addEntry.Activated += (object sender, EventArgs e) => AddEntryWidget();

            MenuItem addTextView = new MenuItem("TextView");
            addTextView.Activated += (object sender, EventArgs e) => AddTextViewWidget();



            MenuItem serverItem = new MenuItem("Server");
            serverItem.Submenu = serverMenu;

            MenuItem loginItem = new MenuItem("Login");
            loginItem.Activated += (object sender, EventArgs e) => LoginScreen();

            MenuItem logoutItem = new MenuItem("Logout");
            logoutItem.Activated += delegate(object sender, EventArgs e)
            {
                username = null;
                password = null;
            };


            fileMenu.Append(newFile);
            fileMenu.Append(openFile);
            fileMenu.Append(saveFile);

            addMenu.Append(addEntry);
            addMenu.Append(addTextView);

            serverMenu.Append(loginItem);
            serverMenu.Append(logoutItem);

            menuBar.Append(file);
            menuBar.Append(addItem);
            menuBar.Append(serverItem);

            #endregion

            vboxWindow.PackStart(menuBar, false, false, 2);
            vboxWindow.Add(globalGrid);

            Add(vboxWindow);

            ShowAll();


        }

        public void AddEntryWidget()
        {
            Entry entry = (Entry)EntryWidget();
            listWidget.Add(entry);

            globalGrid.Attach(MovableWidget(entry), 1, gridNumber, 1, 1);
            gridNumber++;

            ShowAll();
        }

        public void AddTextViewWidget()
        {
            TextView textView = (TextView)TextViewWidget();
            listWidget.Add(textView);

            globalGrid.Attach(MovableWidget(textView), 1, gridNumber, 1, 1);
            gridNumber++;

            ShowAll();
        }



        public Widget TextViewWidget()
        {
            TextView textView = new TextView();
            textView.HeightRequest = 100;
            textView.WidthRequest = 300;
            textView.Visible = true;

            return textView;
        }

        public Widget EntryWidget()
        {
            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 300;
            entry.Buffer.Text = "";

            return entry;
        }

        public void UpdateWorkspace()
        {
            mt.Clear();

            foreach (Widget w in listWidget)
            {
                if (w.GetType() == typeof(Entry))
                {
                    Entry en = (Entry)w;
                    MetaType mtlmt = new MetaType();
                    mtlmt.type = en.GetType();
                    mtlmt.metastring0 = en.Text;
                    mtlmt.metaint0 = en.HeightRequest;
                    mtlmt.metaint1 = en.WidthRequest;

                    mt.Add(mtlmt);
                }
                if (w.GetType() == typeof(TextView))
                {
                    TextView tv = (TextView)w;
                    MetaType mtlmt = new MetaType();
                    mtlmt.type = tv.GetType();
                    mtlmt.metastring0 = tv.Buffer.Text;
                    mtlmt.metaint0 = tv.HeightRequest;
                    mtlmt.metaint1 = tv.WidthRequest;
                    mt.Add(mtlmt);
                }
            }
        }

        void OpenFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        System.IO.Stream myStream = null;
                        System.Windows.Forms.OpenFileDialog filechooser = new System.Windows.Forms.OpenFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.FileName);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.Filename);
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            ClearWindow();

            mt.Clear();
            List<MetaType> mtlmt = new List<MetaType>();
            mtlmt = Import.DeserializeString<List<MetaType>>(casFile);
            mt = mtlmt;
            listWidget.Clear();

            foreach (var item in mt)
            {
                if (item.type == typeof(Entry))
                {
                    Entry entry = new Entry();
                    entry.Text = item.metastring0;
                    entry.HeightRequest = item.metaint0;
                    entry.WidthRequest = item.metaint1;
                    listWidget.Add(entry);
                }
                if (item.type == typeof(TextView))
                {
                    TextView textView = new TextView();
                    textView.Buffer.Text = item.metastring0;
                    textView.HeightRequest = item.metaint0;
                    textView.WidthRequest = item.metaint1;
                    listWidget.Add(textView);
                }
            }

            foreach (Widget item in listWidget)
            {
                globalGrid.Attach(MovableWidget(item), 1, gridNumber, 1, 1);
                gridNumber++;
            }

            ShowAll();
        }

        void SaveFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            Console.WriteLine(mt.Count);

            UpdateWorkspace();

            Console.WriteLine(mt.Count);

            casFile = ImEx.Export.Serialize(mt);
            Console.WriteLine("CAS FILE:::\n" + casFile);

            switch (pid)
            {

                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        System.IO.Stream myStream = null;
                        System.Windows.Forms.SaveFileDialog filechooser = new System.Windows.Forms.SaveFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            System.IO.File.WriteAllText(filechooser.FileName, casFile);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Save File...", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            System.IO.File.WriteAllText(filechooser.Filename, casFile);
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        void ClearWindow()
        {
            listWidget.Clear();

            foreach (Widget item in globalGrid)
            {
                globalGrid.Remove(item);
            }

            gridNumber = 1;
        }

        public Widget MovableWidget(Widget widget)
        {
            Grid grid = new Grid();

            Button buttonMoveUp = new Button("↑");
            buttonMoveUp.HeightRequest = 10;
            buttonMoveUp.WidthRequest = 10;
            buttonMoveUp.Clicked += delegate(object sender, EventArgs e)
            {

                Console.WriteLine(listWidget.Count);

                foreach (Widget item in listWidget)
                {
                    Console.WriteLine("Item in lw: " + item.ToString());
                }

                int ID = listWidget.IndexOf(widget);
                Console.WriteLine("ID: " + ID);
                if (ID >= 1)
                {
                    Console.WriteLine("Inside deligate");
                    Console.WriteLine("Moving ID: " + listWidget.IndexOf(widget));
                    Widget w = listWidget[ID];
                    Console.WriteLine("Got widget");
                    listWidget.RemoveAt(ID);
                    Console.WriteLine("Removed Widget\nInserting widget @ " + ID);
                    listWidget.Insert(ID - 1, w);
                    Console.WriteLine("Inserted Widget");
                    Redraw();
                }

                foreach (Widget item in listWidget)
                {
                    Console.WriteLine("Item in lw: " + item.ToString());
                }


                Console.WriteLine(listWidget.Count);
            };

            Button buttonMoveDown = new Button("↓");
            buttonMoveDown.HeightRequest = 10;
            buttonMoveDown.WidthRequest = 10;
            buttonMoveDown.Clicked += delegate(object sender, EventArgs e)
            {

                Console.WriteLine(listWidget.Count);

                foreach (Widget item in listWidget)
                {
                    Console.WriteLine("Item in lw: " + item.ToString());
                }

                int ID = listWidget.IndexOf(widget);
                Console.WriteLine("ID: " + ID);
                if (ID <= listWidget.Count - 2)
                {
                    Console.WriteLine("Inside deligate");
                    Console.WriteLine("Moving ID: " + listWidget.IndexOf(widget));
                    Widget w = listWidget[ID];
                    Console.WriteLine("Got widget");
                    listWidget.RemoveAt(ID);
                    Console.WriteLine("Removed Widget\nInserting widget @ " + ID);
                    listWidget.Insert(ID + 1, w);
                    Console.WriteLine("Inserted Widget");
                    Redraw();
                }

                foreach (Widget item in listWidget)
                {
                    Console.WriteLine("Item in lw: " + item.ToString());
                }


                Console.WriteLine(listWidget.Count);
            };

            grid.Attach(widget, 1, 1, 1, 2);
            grid.Attach(buttonMoveUp, 2, 1, 1, 1);
            grid.Attach(buttonMoveDown, 2, 2, 1, 1);

            return grid;
        }

        void Redraw()
        {
            UpdateWorkspace();

            listWidget.Clear();

            foreach (Widget w in globalGrid)
            {
                globalGrid.Remove(w);
            }

            foreach (var item in mt)
            {
                if (item.type == typeof(Entry))
                {
                    Entry entry = new Entry();
                    entry.Text = item.metastring0;
                    entry.HeightRequest = item.metaint0;
                    entry.WidthRequest = item.metaint1;
                    listWidget.Add(entry);
                }
                if (item.type == typeof(TextView))
                {
                    TextView textView = new TextView();
                    textView.Buffer.Text = item.metastring0;
                    textView.HeightRequest = item.metaint0;
                    textView.WidthRequest = item.metaint1;
                    listWidget.Add(textView);
                }
            }

            foreach (Widget item in listWidget)
            {
                globalGrid.Attach(MovableWidget(item), 1, gridNumber, 1, 1);
                gridNumber++;
            }

            ShowAll();
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

            int privilege;

            buttonLogin.Clicked += delegate(object sender, EventArgs e)
            {
                username = entryUsername.Text;
                password = entryPassword.Text;

                const string host = "http://localhost:8080/";
                const string command = "Login ";

                var client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                client.Credentials = new NetworkCredential(username, password);

                login LoginHandler = (x, y) => int.Parse(client.UploadString(x, y), System.Globalization.NumberStyles.AllowLeadingSign);
                privilege = LoginHandler(host, command);

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

        /*

        public CASGui()
            : base("CAS.Net gui")
        {

            DeleteEvent += (o, a) => Application.Quit();

            globVarMax = 10;
            globVarMin = 1;
            globVarNum = 2;

            SetSizeRequest(300, 500);
            oVB = new VBox(false, 2);
            iVB = new VBox(false, 2);
            Table table1 = new Table(2, 2, false);
            ScrolledWindow scroll = new ScrolledWindow();

            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 100;

            SetPosition(WindowPosition.Center);

            #region Menuer
            MenuBar mb = new MenuBar();
            Menu filemenu = new Menu();
            Menu addFileMenu = new Menu();
            Menu serverMenu = new Menu();


            MenuItem file = new MenuItem("File");
            file.Submenu = filemenu;

            MenuItem newFile = new MenuItem("New File");
            newFile.Activated += (object sender, EventArgs e) => ClearWindow();

            MenuItem openFile = new MenuItem("Open File");
            openFile.Activated += (o, a) => OpenFile();

            MenuItem saveFile = new MenuItem("Save File");
            saveFile.Activated += (o, a) => SaveFile();

            MenuItem properties = new MenuItem("Properties");
            properties.Activated += (o, a) => OnActivatedProperties(globVarMin, globVarMax, globVarNum);

            MenuItem exit = new MenuItem("Exit");
            exit.Activated += OnActivated;



            MenuItem addItem = new MenuItem("Add Item");
            addItem.Submenu = addFileMenu;

            MenuItem gen = new MenuItem("Generate Assignment");
            gen.Activated += (o, a) => OnActivatedGen();

            MenuItem head = new MenuItem("Add headline");
            head.Activated += (object sender, EventArgs e) => AddHeadlineBox();

            MenuItem text = new MenuItem("Text Field");
            text.Activated += (object sender, EventArgs e) => AddTextBox();



            MenuItem server = new MenuItem("Server");
            server.Submenu = serverMenu;

            MenuItem login = new MenuItem("Login");
            login.Activated += (object sender, EventArgs e) => LoginScreen();

            filemenu.Append(newFile);
            filemenu.Append(openFile);
            filemenu.Append(saveFile);
            filemenu.Append(properties);
            filemenu.Append(exit);

            addFileMenu.Append(gen);
            addFileMenu.Append(head);
            addFileMenu.Append(text);

            serverMenu.Append(login);

            mb.Append(file);
            mb.Append(addItem);
            mb.Append(server);
            #endregion Menuer

            table1.Attach(SetupLabAss(), 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //Assignment
            table1.Attach(entry, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //answer
            table1.Attach(SetupTV(100, 100, ""), 0, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); // MR


            iVB.Add(table1);
            oVB.PackStart(mb, false, false, 8);
            oVB.Add(scroll);
            scroll.Add(iVB);

            Add(oVB);
            ShowAll();
        }

        // Menu to set number minimum, maximum and number of numbers used for autogenerated equations.
        void OnActivatedProperties(int min, int max, int num)
        {
            Window PropertiesWindow = new Window("This is a window");
            PropertiesWindow.SetDefaultSize(200, 200);

            VBox vbox = new VBox(false, 2);
            HBox hbox = new HBox(false, 2);

            // string smin = min.ToString();
            // string smax = max.ToString();
            // string snum = num.ToString();

            Label varMin = new Label("VarMin");
            Label varMax = new Label("varMax");
            Label varNum = new Label("varNum");

            Table table = new Table(2, 4, false);

            table.Attach(varMin, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(varMax, 0, 1, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(varNum, 0, 1, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            SpinButton sbMin = new SpinButton(0, 100000000, 1);
            sbMin.Value = globVarMin;
            sbMin.WidthRequest = 10;
            SpinButton sbMax = new SpinButton(0, 100000000, 1);
            sbMax.Value = globVarMax;
            sbMax.WidthRequest = 10;
            SpinButton sbNum = new SpinButton(2, 5, 1);
            sbNum.Value = globVarNum;
            sbNum.WidthRequest = 10;

            table.Attach(sbMin, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(sbMax, 1, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(sbNum, 1, 2, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            Button ok = new Button("Confirm");
            Button cancel = new Button("Cancel");

            cancel.Clicked += (object sender, EventArgs e) => PropertiesWindow.Destroy();

            ok.Clicked += delegate(object sender, EventArgs e)
            {
                globVarMin = sbMin.ValueAsInt;
                globVarMax = sbMax.ValueAsInt;
                globVarNum = sbNum.ValueAsInt;
                PropertiesWindow.Destroy();
            };

            hbox.Add(cancel);
            hbox.Add(ok);

            vbox.Add(table);
            vbox.Add(hbox);

            PropertiesWindow.Add(vbox);
            PropertiesWindow.ShowAll();
        }

        // Quits the application on call
        void OnActivated(object sender, EventArgs args)
        {
            Application.Quit();
        }

        // Generates new auto-generated equation
        void OnActivatedGen()
        {
            Table table = new Table(2, 2, false);
            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 100;
            entry.Buffer.Text = "";

            table.Attach(SetupLabAss(), 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //Assignment
            table.Attach(entry, 4, 5, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //answer
            table.Attach(SetupTV(100, 100, ""), 0, 5, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); // MR
            iVB.Add(table);
            ShowAll();

        }

        // Adds a oneline headline box
        void AddHeadlineBox()
        {
            Table table = new Table(1, 1, false);
            Entry entry = new Entry();

            entry.HeightRequest = 20;
            entry.WidthRequest = 100;
            entry.Buffer.Text = "";

            table.Attach(entry, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 3, 3);
            iVB.Add(table);
            ShowAll();
        }

        // Adds a textbox
        // Please note that it currently writes all text on one line only.
        void AddTextBox()
        {
            Table table = new Table(1, 1, false);
            TextView textView = new TextView();

            textView.HeightRequest = 100;
            textView.WidthRequest = 200;
            textView.Visible = true;

            table.Attach(textView, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 3, 3);
            iVB.Add(table);
            ShowAll();
        }

        // Generates the calculation for the equation
        public Label SetupLabAss()
        {
            Label labAss = new Label(TaskGen.MakeCalcTask(globVarMin, globVarMax, globVarNum));
            return labAss;
        }

        // Generates textbox
        public TextView SetupTV(int h, int w, string text)
        {
            TextView t = new TextView();
            t.HeightRequest = h;
            t.WidthRequest = w;
            t.Buffer.Text = text;
            t.Visible = true;
            return t;
        }

        void OpenFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        System.IO.Stream myStream = null;
                        System.Windows.Forms.OpenFileDialog filechooser = new System.Windows.Forms.OpenFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.FileName);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.Filename);
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        void SaveFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        System.IO.Stream myStream = null;
                        System.Windows.Forms.SaveFileDialog filechooser = new System.Windows.Forms.SaveFileDialog();
                       
                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            System.IO.File.WriteAllText(filechooser.FileName, casFile);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Save File...", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            System.IO.File.WriteAllText(filechooser.Filename, casFile);
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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

        void ClearWindow()
        {
            foreach (Widget item in iVB)
            {
                iVB.Remove(item);
            }
        }
        */
    }
}