using System;
using Gtk;
using System.Text;

namespace DesktopUI
{
    public class UnderlineToolButton : ToolButton
    {
        TextViewList textviews;

        public UnderlineToolButton(ref TextViewList textviews)
            : base("Underline")
        {
            this.textviews = textviews;

            Clicked += delegate
            {
                OnUnderlineClicked();
            };
        }

        void OnUnderlineClicked()
        {
            foreach (var item in textviews)
            {
                if (item.GetType() == typeof(MovableCasTextView))
                {
                    TextBuffer buffer = (item as MovableCasTextView).textview.Buffer;
                    TextIter startIter, endIter;
                    buffer.GetSelectionBounds(out startIter, out endIter);

                    byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                    string s = Encoding.UTF8.GetString(byteTextView);

                    Console.WriteLine(s);

                    if (s.Contains("<attr name=\"underline\" type=\"PangoUnderline\" value=\"PANGO_UNDERLINE_SINGLE\" />"))
                    {
                        buffer.RemoveTag((item as MovableCasTextView).textview.underlineTag, startIter, endIter);
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableCasTextView).textview.underlineTag, startIter, endIter);
                    }

                }
                else if (item.GetType() == typeof(MovableLockedCasTextView))
                {
                    TextBuffer buffer = (item as MovableLockedCasTextView).textview.Buffer;
                    TextIter startIter, endIter;
                    buffer.GetSelectionBounds(out startIter, out endIter);

                    byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                    string s = Encoding.UTF8.GetString(byteTextView);

                    if (s.Contains("<attr name=\"underline\" type=\"PangoUnderline\" value=\"PANGO_UNDERLINE_LOW\" />"))
                    {
                        buffer.RemoveTag((item as MovableLockedCasTextView).textview.underlineTag, startIter, endIter);
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableLockedCasTextView).textview.underlineTag, startIter, endIter);
                    }
                }

            }

        }
    }
}

