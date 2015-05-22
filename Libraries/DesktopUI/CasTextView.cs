using System;
using System.Collections.Generic;
using System.Text;
using Gtk;

namespace DesktopUI
{
    // The base element for widgets.
    // All cas widgets inherit from this
    public class CasTextView : TextView
    {
        public bool locked = false;

        // Text tags used for formatting text.
        public TextTag boldTag = new TextTag("BoldTag");
        public TextTag italicTag = new TextTag("ItalicTag");
        public TextTag underlineTag = new TextTag("UnderlineTag");

        // Constructor for castextview, adds all tags to the textview
        public CasTextView(string deserializedString, bool locked)
            : base()
        {
            boldTag.Weight = Pango.Weight.Bold;
            Buffer.TagTable.Add(boldTag);

            italicTag.Style = Pango.Style.Italic;
            Buffer.TagTable.Add(italicTag);

            underlineTag.Underline = Pango.Underline.Single;
            Buffer.TagTable.Add(underlineTag);

            WrapMode = WrapMode.WordChar;
            
            CasTextViewSerializer serializer = new CasTextViewSerializer();

            Buffer.Text = deserializedString;
            this.locked = locked;

            ShowAll();
        }

        // A simplified constructor, used for TaskGen.
        public CasTextView(string TaskString)
            : base()
        {
            Buffer.Text = TaskString;
            ShowAll();
        }

        // Can be used to lock widget
        public void LockTextView(bool locked)
        {
            Editable = !locked;
        }
    }
}

