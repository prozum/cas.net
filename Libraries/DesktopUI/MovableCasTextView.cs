using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
    public class MovableCasTextView : Grid
    {
        TextViewList parent;
        public CasTextView textview;
        Button ButtonMoveUp = new Button("↑");
        Button ButtonMoveDown = new Button("↓");
        /* insert arror moving thingy here */

        static int ID = 0;
        public int id_;

        public MovableCasTextView(TextViewList parent, string serializedString, bool teacherCanEdit)
        {
            id_ = ID++;
            this.parent = parent;

            textview = new CasTextView(serializedString, teacherCanEdit);
            textview.WidthRequest = 300;
            textview.HeightRequest = 200;

            ButtonMoveUp.Clicked += delegate
            {
                MoveUp();
            };

            ButtonMoveDown.Clicked += delegate
            {
                MoveDown();
            };

            Attach(textview, 1, 1, 1, 2);
            Attach(ButtonMoveUp, 2, 1, 1, 1);
            Attach(ButtonMoveDown, 2, 2, 1, 1);
        }

        void MoveUp()
        {
            parent.Move(id_, -1);
        }

        void MoveDown()
        {
            parent.Move(id_, 1);
        }
    }
}
