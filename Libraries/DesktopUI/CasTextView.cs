using System;
using System.Collections.Generic;
using System.Text;
using Gtk;

namespace DesktopUI
{
    public class CasTextView : TextView
    {
        public bool teacherEditOnly = false;

        public CasTextView(string SerializedString, bool TeacherCanEdit)
            : base()
        {
            WrapMode = WrapMode.WordChar;
            DeserializeCasTextView(SerializedString);
            teacherEditOnly = TeacherCanEdit;
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
    }
}

