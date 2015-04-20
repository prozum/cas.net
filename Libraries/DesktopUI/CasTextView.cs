using System;
using Gtk;
using System.Text;

namespace DesktopUI
{
    public class CasTextView : Bin
    {
        TextView textView = new TextView();
        TextTag boldTag = new TextTag("BoldTag");

        Toolbar toolbar = new Toolbar();
        ToolButton toolButtonBold = new ToolButton(Stock.Bold);

        public CasTextView()
            : base()
        {
            Grid grid = new Grid();
            toolbar.Add(toolButtonBold);
            textView.Buffer.TagTable.Add(boldTag);
            toolButtonBold.Clicked += (object sender, EventArgs e) =>
            {
                TextIter startIter, endIter;
                TextBuffer buffer = textView.Buffer;
                buffer.GetBounds(out startIter, out endIter);

                byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset("test"), startIter, endIter);
                string s = Encoding.UTF8.GetString(byteTextView);

                Console.WriteLine(s);

                if (s.Contains("<attr name=\"weight\" type=\"gint\" value=\"700\" />"))
                {
                    buffer.RemoveTag(boldTag, startIter, endIter);                
                }
                else
                {
                    buffer.ApplyTag(boldTag, startIter, endIter);
                }

                byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset("test"), startIter, endIter);
                s = Encoding.UTF8.GetString(byteTextView);
//                Console.WriteLine(s);
            };

            grid.Attach(toolbar, 1, 1, 1, 1);
            grid.Attach(textView, 1, 2, 1, 1);
            this.Add(grid);
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
            buffer.TagTable.Add(boldTag);
            byte[] byteTextView = Encoding.UTF8.GetBytes(serializedTextView);
            buffer.Deserialize(buffer, buffer.RegisterDeserializeTagset("test"), ref textIter, byteTextView, (ulong)byteTextView.Length);
        }
    }
}

