using System;
using Gtk;
using TaskGenLib;

namespace Gui.Tests
{
    class CASGui : Window
    {
        VBox oVB;
        VBox iVB;
        int varMin, varMax, varNum;

        public CASGui()
            : base("CAS.Net gui")
        {
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

            MenuItem file = new MenuItem("MenuTest");
            file.Submenu = filemenu;

            MenuItem gen = new MenuItem("Generate Assignment");
            gen.Activated += (o, a) => OnActivatedGen();

            MenuItem exit = new MenuItem("Exit");
            exit.Activated += OnActivated;

            MenuItem properties = new MenuItem("Properties");
            properties.Activated += (o, a) => OnActivatedProperties(1, 10, 2);

            filemenu.Append(properties);
            filemenu.Append(exit);
            filemenu.Append(gen);
            mb.Append(file);
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

        public static void Main(string[] args)
        {
            Application.Init();
            new CASGui();
            Application.Run();
        }

        void OnActivatedProperties(int min, int max, int num)
        {
            Window myWindow = new Window("This is a window");
            myWindow.SetDefaultSize(200, 200);

            VBox vbox = new VBox(false, 2);
            HBox hbox = new HBox(false, 2);

            string smin = min.ToString();
            string smax = max.ToString();
            string snum = num.ToString();

            Label varMin = new Label("VarMin");
            Label varMax = new Label("varMax");
            Label varNum = new Label("varNum");

            Table table = new Table(2, 4, false);

            table.Attach(varMin, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(varMax, 0, 1, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(varNum, 0, 1, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            SpinButton sbMin = new SpinButton(0, 100000000, 1);
            sbMin.WidthRequest = 10;
            SpinButton sbMax = new SpinButton(0, 100000000, 1);
            sbMax.WidthRequest = 10;
            SpinButton sbNum = new SpinButton(0, 5, 1);
            sbNum.WidthRequest = 10;

            table.Attach(sbMin, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(sbMax, 1, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(sbNum, 1, 2, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            Button ok = new Button("Confirm");
            Button cancel = new Button("Cancel");

            cancel.Clicked += delegate
            {
                myWindow.Destroy();
            };

            hbox.Add(cancel);
            hbox.Add(ok);

            vbox.Add(table);
            vbox.Add(hbox);

            myWindow.Add(vbox);
            myWindow.ShowAll();
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

            table.Attach(SetupLabAss(), 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //Assignment
            table.Attach(entry, 4, 5, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //answer
            table.Attach(SetupTV(100, 100, ""), 0, 5, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); // MR
            iVB.Add(table);
            ShowAll();

        }

        public Label SetupLabAss()
        {
            Label labAss = new Label(TaskGen.MakeCalcTask(1, 10, 2));
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