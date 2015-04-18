using System;
using System.Text;
using Gtk;
using Pango;
using System.Collections.Generic;

namespace FancyTextEditor
{
    public class FancyWindow : Window
    {
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

            Toolbar toolbar = new Toolbar();
            grid.Attach(toolbar, 1, 1, 1, 1);

            TextView textview = new TextView();
            grid.Attach(textview, 1, 2, 1, 1);

            TextTag boldTag = new TextTag("BoldTag");
            boldTag.Weight = Weight.Bold;

            TextTag italicTag = new TextTag("ItalicTag");
            italicTag.Style = Pango.Style.Italic;

            TextTag underlineTag = new TextTag("UnderlineTag");
            underlineTag.Underline = Pango.Underline.Single;

            TextBuffer buffer = textview.Buffer;
            buffer.TagTable.Add(boldTag);
            buffer.TagTable.Add(italicTag);
            buffer.TagTable.Add(underlineTag);

            ToolButton toolButtonBold = new ToolButton(Stock.Bold);
            toolbar.Insert(toolButtonBold, 0);
            toolButtonBold.Clicked += delegate
            {

                TextIter startIter, endIter;

                buffer.GetSelectionBounds(out startIter, out endIter);
                byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
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
            };

            ToolButton toolButtonItalic = new ToolButton(Stock.Italic);
            toolbar.Insert(toolButtonItalic, 1);
            toolButtonItalic.Clicked += delegate(object sender, EventArgs e)
            {

                TextIter startIter, endIter;

                buffer.GetSelectionBounds(out startIter, out endIter);
                byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                string s = Encoding.UTF8.GetString(byteTextView);

                Console.WriteLine(s);

                if (s.Contains("<attr name=\"style\" type=\"PangoStyle\" value=\"PANGO_STYLE_ITALIC\" />"))
                {
                    buffer.RemoveTag(italicTag, startIter, endIter);
                }
                else
                {
                    buffer.ApplyTag(italicTag, startIter, endIter);
                }
            };

            ToolButton toolButtonUnderline = new ToolButton(Stock.Underline);
            toolbar.Insert(toolButtonUnderline, 2);
            toolButtonUnderline.Clicked += delegate(object sender, EventArgs e)
            {

                TextIter startIter, endIter;

                buffer.GetSelectionBounds(out startIter, out endIter);
                byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                string s = Encoding.UTF8.GetString(byteTextView);

                Console.WriteLine(s);

                if (s.Contains("<attr name=\"underline\" type=\"PangoUnderline\" value=\"PANGO_UNDERLINE_LOW\" />"))
                {
                    buffer.RemoveTag(underlineTag, startIter, endIter);
                }
                else
                {
                    buffer.ApplyTag(underlineTag, startIter, endIter);
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
    }

}
