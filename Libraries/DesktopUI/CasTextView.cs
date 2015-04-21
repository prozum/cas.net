using System;
using Gtk;
using System.Text;
using System.Collections.Generic;

namespace DesktopUI
{
    public class CasTextView : TextView
    {
        bool teacherEditOnly = false;
        List<Widget> listWidget;

        public CasTextView(string serializedString, bool teacherCanEdit, List<Widget> listWidget)
            : base()
        {
            DeserializeCasTextView(serializedString);
            teacherEditOnly = teacherCanEdit;
            this.listWidget = listWidget;
            ShowAll();
        }

        public byte[] SerializeCasTextView()
        {
            TextIter startIter, endIter;
            Buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = Buffer.Serialize(Buffer, Buffer.RegisterSerializeTagset(null), startIter, endIter);
            string serializedTextView = Convert.ToBase64String(byteTextView);

            Console.WriteLine(serializedTextView);

            return byteTextView;
        }

        public void DeserializeCasTextView(string serializedTextView)
        {
            Console.WriteLine("Input: " + serializedTextView);
            Console.WriteLine("Deserializing...");
            TextIter textIter = Buffer.StartIter;
            byte[] byteTextView = Convert.FromBase64String(serializedTextView);
            Console.WriteLine("Length: " + byteTextView.Length);
            Buffer.Deserialize(Buffer, Buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);
        }

        public void SetTeacherEditOnly(bool isLocked)
        {
            teacherEditOnly = isLocked;
        }

        public Widget GetMovableWidget()
        {
            CasMovableWidget movableWidget = new CasMovableWidget(this, listWidget);
            return movableWidget.GetMovableWidget();
        }
    }
}

