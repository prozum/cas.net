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

        public string SerializeCasTextView()
        {
            TextIter startIter, endIter;
            Buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = Buffer.Serialize(Buffer, Buffer.RegisterSerializeTagset("test"), startIter, endIter);
            string serializedTextView = Encoding.UTF8.GetString(byteTextView);

            Console.WriteLine(serializedTextView);

            return serializedTextView;
        }

        public void DeserializeCasTextView(string serializedTextView)
        {
            TextIter textIter = Buffer.StartIter;
            byte[] byteTextView = Encoding.UTF8.GetBytes(serializedTextView);
            Buffer.Deserialize(Buffer, Buffer.RegisterDeserializeTagset("test"), ref textIter, byteTextView, (ulong)byteTextView.Length);
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

