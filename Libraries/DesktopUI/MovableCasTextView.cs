using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
    // The base movable widget, that all other movable widgets inherit from
    public class MovableCasTextView : Grid
    {
        public CasTextView textview;

        static int ID = 0;
        public int id_;

        // Constructor for movablecastextview
		public MovableCasTextView(string serializedString, bool locked)
        {
            id_ = ID++;

            CasTextViewSerializer serializer = new CasTextViewSerializer();
            string deserializedString = serializer.DeserializeCasTextView(serializedString);

            textview = new CasTextView(deserializedString, locked);
            textview.WidthRequest = 300;
            textview.HeightRequest = 40;

            Attach(textview, 1, 1, 1, 2);
        }

        // Constructor for movablecastextview, used for taskgen
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
