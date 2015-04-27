using System;
using Gtk;

namespace DesktopUI
{
    public class MovableLockedCasTextView : MovableCasTextView
    {
        readonly CheckButton checkButton = new CheckButton("Lock for students");

        public MovableLockedCasTextView(string serializedString, bool locked)
            : base(serializedString, locked)
        {
            checkButton.Toggled += delegate
            {
                textview.locked = !textview.locked;
            };

            Attach(checkButton, 1, 3, 1, 1);
        }
    }
}

