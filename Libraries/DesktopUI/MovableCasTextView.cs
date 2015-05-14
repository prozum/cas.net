using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
    public class MovableCasTextView : Grid
    {
        public CasTextView textview;

        static int ID = 0;
        public int id_;

		public MovableCasTextView(string serializedString, bool locked)
        {
            id_ = ID++;

            textview = new CasTextView(serializedString, locked);
            textview.WidthRequest = 300;
            textview.HeightRequest = 40;

            Attach(textview, 1, 1, 1, 2);
        }

        public MovableCasTextView(string TaskString)
        {
            id_ = ID++;

            textview = new CasTextView(TaskString);
            textview.WidthRequest = 300;
            textview.HeightRequest = 40;

            Attach(textview, 1, 1, 1, 2);
        }
    }
}
