using Gtk;
using ImEx;

namespace GraphicalImportExport
{

    public class MainWindow : Window
    {
        static void Main (string[] args)
        {
            Application.Init ();

            new MainWindow ();

            Application.Run ();
        }

        public MainWindow () : base ("MainWindow")
        {
            string path = "";
            Window w = new Window ("A window");
            OpenFile f = new OpenFile (w, out path);
            w.Add (f);
            SaveFile s = new SaveFile (w, path);
            w.Add (s);
            ShowAll ();

            /*
        // Setup ui
            var textview = new TextView ();
            Add (textview);

            // Setup tag
            var tag = new TextTag ("helloworld-tag");
            tag.Scale = Pango.Scale.XXLarge;
            tag.Style = Pango.Style.Italic;
            tag.Underline = Pango.Underline.Double;
            tag.Foreground = "blue";
            tag.Background = "pink";
            tag.Justification = Justification.Center;
            var buffer = textview.Buffer;
            buffer.TagTable.Add (tag);

            // Insert "Hello world!" into textview buffer
            var insertIter = buffer.StartIter;
            buffer.InsertWithTagsByName (ref insertIter, "Hello World!\n", "helloworld-tag");
            buffer.Insert (ref insertIter, "Simple Hello World!");
            */


            // ShowAll ();
        }

    }

    public class OpenFile : Widget
    {
        public OpenFile (Window w, out string path) // : base
        {
            FileChooserDialog filechooser = new FileChooserDialog ("Open file...", w, FileChooserAction.Open);

            filechooser.AddButton (Stock.Cancel, ResponseType.Cancel);
            filechooser.AddButton (Stock.Open, ResponseType.Ok);

            filechooser.Filter = new FileFilter ();
            filechooser.Filter.AddPattern ("*.cas");

            if (filechooser.Run () == (int)ResponseType.Ok) {
                path = filechooser.Filename;
            } else {
                path = null;
            }

            filechooser.Destroy ();
        }
    }

    public class SaveFile : Widget
    {
        public SaveFile (Window w, string s)
        {
            FileChooserDialog filechooser = new FileChooserDialog ("Save file...", w, FileChooserAction.Save);

            filechooser.AddButton (Stock.Cancel, ResponseType.Cancel);
            filechooser.AddButton (Stock.Save, ResponseType.Ok);

            filechooser.Filter = new FileFilter ();
            filechooser.Filter.AddPattern ("*.cas");

            if (filechooser.Run () == (int)ResponseType.Ok) {
                Export.WriteToCasFile (s, filechooser.Name);
            }

            /*
            if (filechooser.Run () == (int)ResponseType.Ok) {
                filechooser.Name = s;
            } else {

            }
            */

            filechooser.Destroy ();
        }
    }

}
