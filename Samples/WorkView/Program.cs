using System;
using System.Text;
using Gtk;
using System.Collections.Generic;

namespace WorkView
{
    class CASGui : Window
    {
        List<Widget> listWidget = new List<Widget>();
        Grid globalGrid = new Grid();
        TextView textView;
        TextBuffer buffer;
        VBox vboxWindow = new VBox(false, 2);

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
            textView = new TextView();
            buffer = textView.Buffer;

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
            //newFile.Activated += (o, a) => ClearWindow();

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
            //loginItem.Activated += (object sender, EventArgs e) => LoginScreen();

            MenuItem logoutItem = new MenuItem("Logout");
            //logoutItem.Activated += delegate(object sender, EventArgs e)

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
            vboxWindow.Add(textView);

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
            TextIter start, end;
            buffer.GetBounds(out start, out end);

            //TextView textView = new TextView ();
            byte[] bs = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), buffer.StartIter, buffer.EndIter);
            //string s = Encoding.UTF8.GetString(bs, 0, bs.Length);
            string s = Encoding.UTF8.GetString(bs);
            Console.Write(s);
            listWidget.Add(textView);

            //globalGrid.Attach(MovableWidget(textView), 1, gridNumber, 1, 1);
            //gridNumber++;

            textView.Show();
        }

    }
}