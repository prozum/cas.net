using System;
using Gtk;
using DesktopUI;
using Ast;

namespace CAS.NET.Desktop
{
    class MainWindow : Window
    {
        User user = new User();
        Evaluator Eval = new Evaluator();
        DefinitionBox DefBox;

        TextViewList textviews;
        MenuBar menubar = new MenuBar();
        Menu menu = new Menu();
        ServerMenuItem server;
        LoginMenuItem login;
        LogoutMenuItem logout;
        StudentGetAssignmentListMenuItem stdGetAsmList;
        TeacherAddAssignmentMenuItem teaAddAsm;
        TeacherGetAssignmentListMenuItem teaGetAsmList;

        Menu taskgenMenu = new Menu();
        TaskGenAlgMenuItem taskGenMenuAlgItem;
        TaskGenUnitMenuItem taskGenMenuUnitItem;

        GeometMenuItem geometMenuItem;

        Toolbar toolbar = new Toolbar();
        OpenToolButton open;
        SaveToolButton save;
        NewToolButton neo;
        BoldToolButton bold;
        ItalicToolButton italic;
        UnderlineToolButton underline;
        MovableTextViewToolButton movabletextview;
        MovableCalcViewToolButton movablecalcview;
        MovableDrawCanvasToolButton movabledrawcanvas;
        MovableResultToolButton movablecasresult;

        ScrolledWindow scrolledWindow = new ScrolledWindow();

        public MainWindow()
            : base("CAS.NET")
        {
            DeleteEvent += (o, a) => Gtk.Application.Quit();
            
            

            textviews = new TextViewList(user, Eval, this);
            DefBox = new DefinitionBox(Eval);

            // Initiating menu elements
            server = new ServerMenuItem();
            login = new LoginMenuItem(user, menu);
            logout = new LogoutMenuItem(user, menu);
            stdGetAsmList = new StudentGetAssignmentListMenuItem(user, textviews);
            teaAddAsm = new TeacherAddAssignmentMenuItem(user, textviews);
            teaGetAsmList = new TeacherGetAssignmentListMenuItem(user, textviews);

            taskGenMenuAlgItem = new TaskGenAlgMenuItem(textviews);
            taskGenMenuUnitItem = new TaskGenUnitMenuItem(textviews);


            // Adding elements to menu
            server.Submenu = menu;
            menu.Append(login);
            menu.Append(logout);
            menu.Append(stdGetAsmList);
            menu.Append(teaAddAsm);
            menu.Append(teaGetAsmList);


            TaskGenMenuItem tgmi = new TaskGenMenuItem(textviews);
            tgmi.Submenu = taskgenMenu;
            taskgenMenu.Append(taskGenMenuAlgItem);
            taskgenMenu.Append(taskGenMenuUnitItem);

            //Dårlig 
            Menu bob = new Menu();
            geometMenuItem = new GeometMenuItem(textviews);
            geometMenuItem.Submenu = bob;

            menubar.Append(server);
            menubar.Append(tgmi);
            menubar.Append(geometMenuItem);

            //menubar.Append(taskGenMenuAlgItem);
            //menubar.Append(taskGenMenuUnitItem);

            open = new OpenToolButton(textviews, ref user);
            save = new SaveToolButton(textviews);
            neo = new NewToolButton(textviews);

            SeparatorToolItem separator1 = new SeparatorToolItem();

            bold = new BoldToolButton(ref textviews);
            italic = new ItalicToolButton(ref textviews);
            underline = new UnderlineToolButton(ref textviews);

            SeparatorToolItem separator2 = new SeparatorToolItem();

            movabletextview = new MovableTextViewToolButton(ref textviews);
            movablecalcview = new MovableCalcViewToolButton(ref textviews);
            movabledrawcanvas = new MovableDrawCanvasToolButton(ref textviews);
            movablecasresult = new MovableResultToolButton(ref textviews);

            toolbar.Add(open);
            toolbar.Add(save);
            toolbar.Add(neo);
            toolbar.Add(separator1);
            toolbar.Add(bold);
            toolbar.Add(italic);
            toolbar.Add(underline);
            toolbar.Add(separator2);
            toolbar.Add(movabletextview);
            toolbar.Add(movablecalcview);
            toolbar.Add(movabledrawcanvas);
            toolbar.Add(movablecasresult);

            VBox vbox = new VBox();
            
            vbox.PackStart(menubar, false, false, 2);
            vbox.PackStart(toolbar, false, false, 2);
            scrolledWindow.Add(textviews);
            vbox.Add(scrolledWindow);
            vbox.Add(DefBox);

            Add(vbox);
            SetSizeRequest(600, 600);
            ShowAll();

            // Rehiding elements not ment to be shown at start, as the
            // user is currently not logged in.
            foreach (Widget w in menu)
            {
                if (w.GetType() == typeof(StudentGetAssignmentListMenuItem)
                    || w.GetType() == typeof(TeacherAddAssignmentMenuItem)
                    || w.GetType() == typeof(TeacherGetAssignmentListMenuItem)
                    || w.GetType() == typeof(LogoutMenuItem))
                {
                    w.Hide();
                }
                else if (w.GetType() == typeof(LoginMenuItem))
                {
                    w.Show();
                }
            }

            GLib.Timeout.Add (2000, new GLib.TimeoutHandler (DefBoxUpdate));
        }

        public bool DefBoxUpdate()
        {
            DefBox.Update();
            return true;
        }
    }
}