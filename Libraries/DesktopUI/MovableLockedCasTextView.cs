using System;
using Gtk;

namespace DesktopUI
{
    public class MovableLockedCasTextView : MovableCasTextView
    {
        CheckButton checkButton = new CheckButton("Lock for students");

        public MovableLockedCasTextView(string serializedString, bool teacherCanEdit)
            : base(serializedString, teacherCanEdit)
        {
            checkButton.Toggled += delegate
            {
                textview.locked = !textview.locked;
            };

            Attach(checkButton, 1, 3, 1, 1);
        }
    }
}

