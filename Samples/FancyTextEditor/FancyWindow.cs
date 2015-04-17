using System;
using System.Text;
using Gtk;
using Pango;
using System.Collections.Generic;

namespace FancyTextEditor
{
    public class FancyWindow : Window
    {
        TextIter startIter, endIter;

        static void Main(string[] args)
        {
            Application.Init();
            new FancyWindow();
            Application.Run();
        }

        public FancyWindow()
            : base("A Fancy Text Editor (TM)")
        {

            DeleteEvent += (o, a) => Application.Quit();

            Grid grid = new Grid();
            Add(grid);

            // Setup ui
            Toolbar toolbar = new Toolbar();
            grid.Attach(toolbar, 1, 1, 1, 1);

            TextView textview = new TextView();
            grid.Attach(textview, 1, 2, 1, 1);

            TextTag textTag = new TextTag("TextTag");

            TextBuffer buffer = textview.Buffer;
            buffer.TagTable.Add(textTag);

            TextIter textIter = buffer.StartIter;
//            buffer.InsertWithTagsByName(ref textIter, "Some text", "TextTag");

            ToolButton toolButtonBold = new ToolButton(Stock.Bold);
            toolbar.Insert(toolButtonBold, 0);
            toolButtonBold.Clicked += delegate
            {
                buffer.GetSelectionBounds(out startIter, out endIter);
                byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                string s = Encoding.UTF8.GetString(byteTextView);

                if (s.Contains("<attr name=\"weight\" type=\"gint\" value=\"700\" />"))
                {
                    textTag.Weight = Pango.Weight.Normal;
                    buffer.ApplyTag(textTag, startIter, endIter);
                }
                else
                {
                    textTag.Weight = Pango.Weight.Bold;
                    buffer.ApplyTag(textTag, startIter, endIter);
                }
            };

            ToolButton toolButtonItalic = new ToolButton(Stock.Italic);
            toolbar.Insert(toolButtonItalic, 1);
            toolButtonItalic.Clicked += delegate(object sender, EventArgs e)
            {
                buffer.GetSelectionBounds(out startIter, out endIter);
                byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                string s = Encoding.UTF8.GetString(byteTextView);

                if (s.Contains("<attr name=\"style\" type=\"PangoStyle\" value=\"PANGO_STYLE_ITALIC\" />"))
                {
                    textTag.Style = Pango.Style.Normal;
                    buffer.ApplyTag(textTag, startIter, endIter);
                }
                else
                {
                    textTag.Style = Pango.Style.Italic;
                    buffer.ApplyTag(textTag, startIter, endIter);
                }
            };

            ToolButton toolButtonUnderline = new ToolButton(Stock.Underline);
            toolbar.Insert(toolButtonUnderline, 2);
            toolButtonUnderline.Clicked += delegate(object sender, EventArgs e)
            {
                buffer.GetSelectionBounds(out startIter, out endIter);
                byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                string s = Encoding.UTF8.GetString(byteTextView);

                if (s.Contains("<attr name=\"underline\" type=\"PangoUnderline\" value=\"PANGO_UNDERLINE_LOW\" />"))
                {
                    textTag.Underline = Pango.Underline.None;
                    buffer.ApplyTag(textTag, startIter, endIter);
                }
                else
                {
                    textTag.Underline = Pango.Underline.Low;
                    buffer.ApplyTag(textTag, startIter, endIter);
                }
            };

            SeparatorToolItem separator = new SeparatorToolItem();
            toolbar.Insert(separator, 3);

            textview.HeightRequest = 400;
            textview.WidthRequest = 600;

            /* Setup tag
            TextTag tag = new TextTag("helloworld-tag");
            tag.Scale = Pango.Scale.XXLarge;
            tag.Style = Pango.Style.Italic;
            tag.Underline = Pango.Underline.Double;
            tag.Foreground = "blue";
            tag.Background = "pink";
            tag.Justification = Justification.Center;
            TextBuffer buffer = textview.Buffer;
            buffer.TagTable.Add(tag);


            // Insert "Hello world!" into textview buffer
            TextIter insertIter = buffer.StartIter;
            buffer.InsertWithTagsByName(ref insertIter, "Hello World!\n", "helloworld-tag");
            buffer.Insert(ref insertIter, "Simple Hello World!");

            */

            ShowAll();
        }

        bool NotBold(string str)
        {
            if (str.Contains("<attr name=\"weight\" type=\"gint\" value=\"700\" />"))
            {
                return false;
            }
            return true;
        }
    }

}
