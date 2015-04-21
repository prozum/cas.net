using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using System.Text;
using FileOperation;

namespace DesktopUI
{
    public class SettingsToolbar : Toolbar
    {
        Grid grid;
        int GridNumber;
        List<MetaType> meta;
        List<Widget> widgets;
        Window window;

        public SettingsToolbar(ref Grid grid, ref int GridNumber, ref List<MetaType> meta, ref List<Widget> widgets, Window window)
        {
            this.grid = grid;
            this.GridNumber = GridNumber;
            this.meta = meta;
            this.widgets = widgets;
            this.window = window;

            ToolButton toolButtonNew = new ToolButton(Stock.New);
            Insert(toolButtonNew, 0);
            toolButtonNew.Clicked += delegate
            {
                ClearWindow();
            };

            ToolButton toolButtonOpen = new ToolButton(Stock.Open);
            Insert(toolButtonOpen, 1);
            toolButtonOpen.Clicked += delegate
            {
                OpenFile();
            };

            ToolButton toolButtonSave = new ToolButton(Stock.Save);
            Insert(toolButtonSave, 2);
            toolButtonSave.Clicked += delegate
            {
                SaveFile();
            };

            toolButtonNew.Show();
            toolButtonOpen.Show();
            toolButtonSave.Show();
        }

        void OpenFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

			File fileop = new File();
			string file = fileop.Open(pid, window);

            ClearWindow();

            meta.Clear();
            List<MetaType> mtlmt = new List<MetaType>();
            mtlmt = Import.DeserializeString<List<MetaType>>(file);

            Console.WriteLine("Writing elements importet");

            foreach (var item in mtlmt)
            {
                Console.WriteLine("type: " + item.type);// + " --- ::: --- " + item.metastring);
                Console.WriteLine("metastring" + item.metastring);
            }

            meta = mtlmt;
            widgets.Clear();

            Console.WriteLine("Recreating listWidget in OpenFile");

            RecreateListWidget();

            Console.WriteLine("Elements in listWidget" + widgets.Count);

            foreach (Widget item in widgets)
            {
                grid.Attach(((CasTextView)item).GetMovableWidget(), 1, GridNumber, 1, 1);
                GridNumber++;

                Console.WriteLine("Added widget::: " + item);
            }

            window.ShowAll();
        }

        void SaveFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            Console.WriteLine(meta.Count);

            UpdateWorkspace();

            Console.WriteLine(meta.Count);

            string file = Export.Serialize(meta);
            Console.WriteLine("CAS FILE:::\n" + file);

			File fileop = new File();
			fileop.Save(pid, window, file);
        }

        public void UpdateWorkspace()
        {
            Console.WriteLine("Updating workspace");

            meta.Clear();

            foreach (Widget w in widgets)
            {
                if (w.GetType() == typeof(Entry))
                {
                    Entry en = (Entry)w;
                    MetaType mtlmt = new MetaType();
                    mtlmt.type = en.GetType();
                    mtlmt.metastring = en.Text;

                    meta.Add(mtlmt);
                }
                if (w.GetType() == typeof(TextView))
                {

                    TextView tv = (TextView)w;
                    MetaType mtlmt = new MetaType();
                    TextBuffer buffer = tv.Buffer;
                    TextIter startIter, endIter;
                    buffer.GetBounds(out startIter, out endIter);
                    mtlmt.type = tv.GetType();
                    byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                    mtlmt.metastring = Convert.ToBase64String(byteTextView);

                    meta.Add(mtlmt);
                }
                if (w.GetType() == typeof(CasTextView))
                {
                    CasTextView ctv = (CasTextView)w;
                    MetaType mtlmt = new MetaType();
                    mtlmt.type = ctv.GetType();
                    byte[] byteTextView = ctv.SerializeCasTextView();
                    mtlmt.metastring = Convert.ToBase64String(byteTextView);
                    Console.WriteLine("Metastring: " + mtlmt.metastring);

                    meta.Add(mtlmt);
                }

                Console.WriteLine(w.GetType().ToString());
            }
        }

        public void ClearWindow()
        {
            widgets.Clear();

            foreach (Widget item in grid)
            {
                grid.Remove(item);
            }

            GridNumber = 1;
        }

		public void RecreateListWidget()
        {
            Console.WriteLine("Recreating listWidget: " + meta.Count);
            foreach (var item in meta)
            {
                if (item.type == typeof(Entry))
                {
                    Entry entry = new Entry();
                    entry.Text = item.metastring;

                    widgets.Add(entry);
                }
                if (item.type == typeof(TextView))
                {
                    byte[] byteTextView = Convert.FromBase64String(item.metastring);

                    TextView textView = new TextView();
                    TextBuffer buffer = textView.Buffer;
                    TextIter textIter = buffer.StartIter;

                    buffer.Deserialize(buffer, buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);

                    widgets.Add(textView);
                }
                if (item.type == typeof(DesktopUI.CasTextView))
                {
                    CasTextView ctv = new CasTextView("", true, widgets);
                    Console.WriteLine("Meta: " + item.metastring + " :End Meta");
                    ctv.DeserializeCasTextView(item.metastring);

                    widgets.Add(ctv);
                }
                Console.WriteLine("Type: " + item.type);
            }
        }
    }
}

