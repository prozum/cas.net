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
            Buffer.Text = DeserializeCasTextView(SerializedString);
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
        public string SerializeCasTextView(TextView serializableTextView)
        {
            TextIter startIter, endIter;
            serializableTextView.Buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = serializableTextView.Buffer.Serialize(serializableTextView.Buffer, serializableTextView.Buffer.RegisterSerializeTagset(null), startIter, endIter);
            string serializedTextView = Convert.ToBase64String(byteTextView);

            Console.WriteLine(serializableTextView.Buffer.Text);

            return serializedTextView;
        }

        // A method for deserializing the content serialized in the above method.
        public string DeserializeCasTextView(string serializedTextView)
        {
            TextView deserializableTextView = new TextView();
            TextIter textIter = deserializableTextView.Buffer.StartIter;
            byte[] byteTextView = Convert.FromBase64String(serializedTextView);
            deserializableTextView.Buffer.Deserialize(deserializableTextView.Buffer, deserializableTextView.Buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);

            Console.WriteLine(deserializableTextView.Buffer.Text);
            
            return deserializableTextView.Buffer.Text;
        }

        // Can be used to lock widget
        public void LockTextView(bool locked)
        {
            Editable = !locked;
        }
    }
}

