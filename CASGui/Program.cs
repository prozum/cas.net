using System;
using Gtk;


namespace Gui.Tests
{
    class CASGui : Window
    {
        VBox oVB;
        VBox iVB;

        public CASGui() : base("CAS.Net gui")
        {
            oVB = new VBox(false, 2);
            iVB = new VBox(false, 2);
            Label lbl = new Label("hej");
            Table table1 = new Table(2, 2, false);
            ScrolledWindow scroll = new ScrolledWindow();

            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 100;

            SetPosition(WindowPosition.Center);

            #region Menuer
            MenuBar mb = new MenuBar();
            Menu filemenu = new Menu();

            MenuItem file = new MenuItem("MenuTest");
            file.Submenu = filemenu;

            MenuItem gen = new MenuItem("Generate Assignment");
            gen.Activated += delegate
            {
                //OnActivatedGen();
            };

            MenuItem exit = new MenuItem("Exit");
            exit.Activated += OnActivated;
            filemenu.Append(exit);
            filemenu.Append(gen);
            mb.Append(file);
            #endregion Menuer

            table1.Attach(SetupLabAss("3+4"), 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //Assignment
            table1.Attach(entry, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //answer
            table1.Attach(SetupTV(100, 100, ""), 0, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); // MR

            gen.Activated += (o, a) => OnActivatedGen();

            iVB.Add(table1);
            oVB.PackStart(mb, false, false, 8);
            oVB.Add(scroll);
            scroll.Add(iVB);
            
            Add(oVB);
            ShowAll();
        }

        public static void Main(string[] args)
        {
            Application.Init();
            new CASGui();
            Application.Run();
        }

        void OnActivated(object sender, EventArgs args)
        {
            Application.Quit();
        }

        void OnActivatedGen()
        {
            Table table = new Table(2, 2, false);
            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 100;
            entry.Buffer.Text = "";

            table.Attach(SetupLabAss("3+4"), 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //Assignment
            table.Attach(entry, 4, 5, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //answer
            table.Attach(SetupTV(100, 100, ""), 0, 5, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); // MR
            iVB.Add(table);
            ShowAll();

        }

        public Label SetupLabAss(string ass)
        {
            Label labAss = new Label(ass);
            return labAss;
        }

        public TextView SetupTV(int h, int w, string text)
        {
            TextView t = new TextView();
            t.HeightRequest = h;
            t.WidthRequest = w;
            t.Buffer.Text = text;
            t.Visible = true;
            return t;
        }
    }
}


