using System;
using Gtk;

namespace DesktopUI
{
    public class TeacherGetCompletedListMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public TeacherGetCompletedListMenuItem(ref User user, ref TextViewList textviews)
            : base("Get List of Completed Students")
        {
            this.user = user;
            this.textviews = textviews;

            this.Activated += delegate
            {
				
            };
        }

        void OnClicked()
        {
            TeacherGetCompletedListWindow window = new TeacherGetCompletedListWindow(ref user, ref textviews);
        }
    }
}

