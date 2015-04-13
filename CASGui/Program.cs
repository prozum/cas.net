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

            iVB.Add(lbl);
            ScrolledWindow scroll = new ScrolledWindow();

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

            oVB.Add(mb);
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
    }
}

