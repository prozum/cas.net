using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
    public class MovableCasTextView : Grid
    {
        TextViewList parent;
        public CasTextView textview;
        /* insert arror moving thingy here */

        static int ID = 0;
        public int id_;

        public MovableCasTextView(string serializedString, bool locked)
        {
            id_ = ID++;

            textview = new CasTextView(serializedString, locked);
            textview.WidthRequest = 300;
            textview.HeightRequest = 200;

            Attach(textview, 1, 1, 1, 2);
        }
    }
}
