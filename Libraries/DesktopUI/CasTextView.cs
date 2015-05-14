using System;
using System.Collections.Generic;
using System.Text;
using Gtk;

namespace DesktopUI
{
    public class CasTextView : TextView
    {
        public bool locked = false;

        public TextTag boldTag = new TextTag("BoldTag");
        public TextTag italicTag = new TextTag("ItalicTag");
        public TextTag underlineTag = new TextTag("UnderlineTag");

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

        public CasTextView(string TaskString)
            : base()
        {
            Buffer.Text = TaskString;
            ShowAll();
        }

        public string SerializeCasTextView()
        {
            TextIter startIter, endIter;
            Buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = Buffer.Serialize(Buffer, Buffer.RegisterSerializeTagset(null), startIter, endIter);
            string serializedTextView = Convert.ToBase64String(byteTextView);

            return serializedTextView;
        }

        public void DeserializeCasTextView(string serializedTextView)
        {
            TextIter textIter = Buffer.StartIter;
            byte[] byteTextView = Convert.FromBase64String(serializedTextView);
            Buffer.Deserialize(Buffer, Buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);
        }

        public void LockTextView(bool locked)
        {
            Console.WriteLine(locked);
            Editable = !locked;
        }
    }
}

