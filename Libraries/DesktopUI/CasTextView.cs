using System;
using System.Collections.Generic;
using System.Text;
using Gtk;

namespace DesktopUI
{
    // The base element for widgets.
    // All cas widgets inherit from this
    // A simple widget for writing and formatting text
    public class CasTextView : TextView
    {
        public bool locked = false;

        // Text tags used for formatting text.
        public TextTag boldTag = new TextTag("BoldTag");
        public TextTag italicTag = new TextTag("ItalicTag");
        public TextTag underlineTag = new TextTag("UnderlineTag");

        public CasTextView(string serializedString, bool locked)
            : base()
        {
            boldTag.Weight = Pango.Weight.Bold;
            Buffer.TagTable.Add(boldTag);

            italicTag.Style = Pango.Style.Italic;
            Buffer.TagTable.Add(italicTag);

            underlineTag.Underline = Pango.Underline.Single;
            Buffer.TagTable.Add(underlineTag);

            WrapMode = WrapMode.WordChar;
            DeserializeCasTextView(serializedString);
            this.locked = locked;

            ShowAll();
        }

        // This simplified constructor is currently only used by taskgen
        public CasTextView(string TaskString)
            : base()
        {
            Buffer.Text = TaskString;
            ShowAll();
        }

        // A method for serializing the content of the TextView, making it easy to save to file an server
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

