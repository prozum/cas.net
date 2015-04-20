using System;
using Gtk;
using System.Text;
using System.Collections.Generic;

namespace DesktopUI
{
    public class CasTextView : Bin
    {
        bool teacherEditOnly = false;
        TextView textView = new TextView();
        Type realType;
        List<Widget> listWidget;
        Gdk.Window window;

        public CasTextView(string serializedString, bool teacherCanEdit, List<Widget> listWidget)
            : base()
        {
            DeserializeCasTextView(serializedString);
            teacherEditOnly = teacherCanEdit;
            realType = typeof(CasTextView);
            this.listWidget = listWidget;
            this.Window = window;
            this.Add(textView);
        }

        public string SerializeCasTextView()
        {
            TextBuffer buffer = textView.Buffer;
            TextIter startIter, endIter;
            buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset("test"), startIter, endIter);
            string serializedTextView = Encoding.UTF8.GetString(byteTextView);

            Console.WriteLine(serializedTextView);

            return serializedTextView;
        }

        public void DeserializeCasTextView(string serializedTextView)
        {
            TextBuffer buffer = textView.Buffer;
            TextIter textIter = buffer.StartIter;
            byte[] byteTextView = Encoding.UTF8.GetBytes(serializedTextView);
            buffer.Deserialize(buffer, buffer.RegisterDeserializeTagset("test"), ref textIter, byteTextView, (ulong)byteTextView.Length);
        }

        public void SetTeacherEditOnly(bool isLocked)
        {
            teacherEditOnly = isLocked;
        }

        public Widget GetMovableWidget()
        {
            CasMovableWidget movableWidget = new CasMovableWidget(textView, listWidget);
            return movableWidget.GetMovableWidget();
        }

        public Type GetRealType()
        {
            return realType;
        }
    }
}

