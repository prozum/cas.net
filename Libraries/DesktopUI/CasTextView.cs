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
        public CasTextView(string SerializedString, bool locked)
            : base()
        {
            boldTag.Weight = Pango.Weight.Bold;
            Buffer.TagTable.Add(boldTag);

            italicTag.Style = Pango.Style.Italic;
            Buffer.TagTable.Add(italicTag);

            underlineTag.Underline = Pango.Underline.Single;
            Buffer.TagTable.Add(underlineTag);

            WrapMode = WrapMode.WordChar;
            DeserializeCasTextView(SerializedString);
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

        // A method for serializing the content of a textview buffer, allowing it to be saved in formatted form.
        // Is converted to Base64 to avoid problems with JSON.
        public string SerializeCasTextView()
        {
            TextIter startIter, endIter;
            Buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = Buffer.Serialize(Buffer, Buffer.RegisterSerializeTagset(null), startIter, endIter);
            string serializedTextView = Convert.ToBase64String(byteTextView);

            return serializedTextView;
        }

        // A method for deserializing the content serialized in the above method.
        public void DeserializeCasTextView(string serializedTextView)
        {
            TextIter textIter = Buffer.StartIter;
            byte[] byteTextView = Convert.FromBase64String(serializedTextView);
            Buffer.Deserialize(Buffer, Buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);
        }

        // Can be used to lock widget
        public void LockTextView(bool locked)
        {
            Editable = !locked;
        }
    }
}

