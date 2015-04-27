using System;
using Gtk;
using System.Text;

namespace DesktopUI
{
    public class BoldToolButton : ToolButton
    {
        TextViewList textviews;

        public BoldToolButton(ref TextViewList textviews)
            : base("Bold")
        {
            this.textviews = textviews;

            Clicked += delegate
            {
                OnBoldClicked();
            };
        }

        void OnBoldClicked()
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

                    if (s.Contains("<attr name=\"weight\" type=\"gint\" value=\"700\" />"))
                    {
                        buffer.RemoveTag((item as MovableCasTextView).textview.tag, startIter, endIter);                
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableCasTextView).textview.tag, startIter, endIter);
                    }

                }
            }
        }
    }
}

