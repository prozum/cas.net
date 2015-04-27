using System;
using Gtk;
using System.Text;

namespace DesktopUI
{
    public class ItalicToolButton : ToolButton
    {
        TextViewList textviews;

        public ItalicToolButton(ref TextViewList textviews)
            : base("Italic")
        {
            this.textviews = textviews;

            Clicked += delegate
            {
                OnItalicClicked();
            };
        }

        void OnItalicClicked()
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

                    if (s.Contains("<attr name=\"style\" type=\"PangoStyle\" value=\"PANGO_STYLE_ITALIC\" />"))
                    {
                        buffer.RemoveTag((item as MovableCasTextView).textview.italicTag, startIter, endIter);
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableCasTextView).textview.italicTag, startIter, endIter);
                    }

                }
                else if (item.GetType() == typeof(MovableLockedCasTextView))
                {
                    TextBuffer buffer = (item as MovableLockedCasTextView).textview.Buffer;
                    TextIter startIter, endIter;
                    buffer.GetSelectionBounds(out startIter, out endIter);

                    byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                    string s = Encoding.UTF8.GetString(byteTextView);

                    if (s.Contains("<attr name=\"style\" type=\"PangoStyle\" value=\"PANGO_STYLE_ITALIC\" />"))
                    {
                        buffer.RemoveTag((item as MovableLockedCasTextView).textview.italicTag, startIter, endIter);
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableLockedCasTextView).textview.italicTag, startIter, endIter);
                    }
                }

            }
        }
    }
}

