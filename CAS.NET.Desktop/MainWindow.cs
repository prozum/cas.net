using System;
using Gtk;

//using TaskGenLib;
//using System.Collections.Generic;
//using ImEx;
//using System.Net;
//using System.Text;
using DesktopUI;

namespace CAS.NET.Desktop
{
    class MainWindow : Window
    {
        User user = new User();

        TextViewList textviews;
        MenuBar menubar = new MenuBar();
        Menu menu = new Menu();
        ServerMenuItem server;
        LoginMenuItem login;
        LogoutMenuItem logout;
        StudentAddCompletedMenuItem stdAddCom;
        StudentGetAssignmentMenuItem stdGetAsm;
        StudentGetAssignmentListMenuItem stdGetAsmList;
        StudentGetFeedbackMenuItem stdGetFee;
        TeacherAddAssignmentMenuItem teaAddAsm;
        TeacherAddFeedbackMenuItem teaAddFee;
        TeacherGetAssignmentListMenuItem teaGetAsmList;
        TeacherGetCompletedListMenuItem teaGetComList;
        TeacherGetCompletedMenuItem teaGetCom;

        Toolbar toolbar = new Toolbar();
        OpenToolButton open;
        SaveToolButton save;
        NewToolButton neo;
        BoldToolButton bold;
        ItalicToolButton italic;
        UnderlineToolButton underline;

        ScrolledWindow scrolledWindow = new ScrolledWindow();

        public MainWindow()
            : base("CAS.NET")
        {
            DeleteEvent += (o, a) => Application.Quit();

            textviews = new TextViewList(ref user);

            // Initiating menu elements
            server = new ServerMenuItem();
            login = new LoginMenuItem(ref user, menu);
            logout = new LogoutMenuItem(ref user, ref menu);
            stdAddCom = new StudentAddCompletedMenuItem(ref user, ref textviews);
            stdGetAsm = new StudentGetAssignmentMenuItem(ref user, ref textviews);
            stdGetAsmList = new StudentGetAssignmentListMenuItem(ref user, ref textviews);
            stdGetFee = new StudentGetFeedbackMenuItem(ref user, ref textviews);
            teaAddAsm = new TeacherAddAssignmentMenuItem(ref user, ref textviews);
            teaAddFee = new TeacherAddFeedbackMenuItem(ref user, ref textviews);
            teaGetAsmList = new TeacherGetAssignmentListMenuItem(ref user, ref textviews);
            teaGetComList = new TeacherGetCompletedListMenuItem(ref user, ref textviews);
            teaGetCom = new TeacherGetCompletedMenuItem(ref user, ref textviews);

            // Adding elements to menu
            server.Submenu = menu;
            menu.Append(login);
            menu.Append(logout);
            menu.Append(stdAddCom);
            menu.Append(stdGetAsm);
            menu.Append(stdGetAsmList);
            menu.Append(stdGetFee);
            menu.Append(teaAddAsm);
            menu.Append(teaAddFee);
            menu.Append(teaGetAsmList);
            menu.Append(teaGetComList);
            menu.Append(teaGetCom);

            menubar.Append(server);

            open = new OpenToolButton(textviews, ref user);
            save = new SaveToolButton(textviews);
            neo = new NewToolButton(textviews);

            SeparatorToolItem separator1 = new SeparatorToolItem();

            bold = new BoldToolButton(ref textviews);
            italic = new ItalicToolButton(ref textviews);
            underline = new UnderlineToolButton(ref textviews);

            toolbar.Add(open);
            toolbar.Add(save);
            toolbar.Add(neo);
            toolbar.Add(separator1);
            toolbar.Add(bold);
            toolbar.Add(italic);
            toolbar.Add(underline);

            VBox vbox = new VBox();

            vbox.PackStart(menubar, false, false, 2);
            vbox.PackStart(toolbar, false, false, 2);
            scrolledWindow.Add(textviews);
            vbox.Add(scrolledWindow);

            Add(vbox);

            SetSizeRequest(600, 600);

            ShowAll();

            // Rehiding elements not ment to be shown at boot, as the
            // user is currently not logged in.
            foreach (Widget w in menu)
            {
                if (w.GetType() == typeof(StudentAddCompletedMenuItem)
                    || w.GetType() == typeof(StudentGetAssignmentListMenuItem)
                    || w.GetType() == typeof(StudentGetAssignmentMenuItem)
                    || w.GetType() == typeof(StudentGetFeedbackMenuItem)
                    || w.GetType() == typeof(TeacherAddAssignmentMenuItem)
                    || w.GetType() == typeof(TeacherAddFeedbackMenuItem)
                    || w.GetType() == typeof(TeacherGetAssignmentListMenuItem)
                    //|| w.GetType() == typeof(TeacherGetCompletedListMenuItem)
                    || w.GetType() == typeof(TeacherGetCompletedMenuItem)
                    || w.GetType() == typeof(LogoutMenuItem))
                {
                    w.Hide();
                }
                else if (w.GetType() == typeof(LoginMenuItem))
                {
                    w.Show();
                }
            }
        }
    }
}