using System;
using Gtk;
using TaskGenLib;
using System.Collections.Generic;
using ImEx;

namespace WorkView
{
    class CASGui : Window
    {
        List<Widget> listWidget = new List<Widget>();
        List<MetaType> mt = new List<MetaType>();
        Grid globalGrid = new Grid();
        int gridNumber = 1;
        VBox vboxWindow = new VBox(false, 2);
        string casFile = "";
        string username, password;

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            new CASGui();
            Application.Run();
        }

        public CASGui()
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
            //openFile.Activated += (o, a) => OpenFile();

            MenuItem saveFile = new MenuItem("Save File");
            //saveFile.Activated += (o, a) => SaveFile();



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
            Entry entry =  new Entry ();
            listWidget.Add(entry);

            //globalGrid.Attach(MovableWidget(entry), 1, gridNumber, 1, 1);
            //gridNumber++;

            entry.Show();
        }

        public void AddTextViewWidget()
        {
            TextView textView = new TextView ();
            listWidget.Add(textView);

            //globalGrid.Attach(MovableWidget(textView), 1, gridNumber, 1, 1);
            //gridNumber++;

            textView.Show();
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

        void ClearWindow()
        {
            listWidget.Clear();

            foreach (Widget item in globalGrid)
            {
                globalGrid.Remove(item);
            }

            gridNumber = 1;
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
                //globalGrid.Attach(MovableWidget(item), 1, gridNumber, 1, 1);
                gridNumber++;
            }

            ShowAll();
        }
    }
}