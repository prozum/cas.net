using System;
using Gtk;
using ImEx;

namespace LibUI
{
    public static class UpdateHandling
    {
        public static void UpdateWorkspace()
        {
            GlobalVar.listMeta.Clear();

            foreach (Widget w in GlobalVar.listWidget)
            {
                if (w.GetType() == typeof(Entry))
                {
                    Entry en = (Entry)w;
                    MetaType mtlmt = new MetaType();
                    mtlmt.type = en.GetType();
                    mtlmt.metastring = en.Text;

                    GlobalVar.listMeta.Add(mtlmt);
                }
                if (w.GetType() == typeof(TextView))
                {
                    TextView tv = (TextView)w;
                    MetaType mtlmt = new MetaType();
                    mtlmt.type = tv.GetType();
                    mtlmt.metastring = tv.Buffer.Text;
                    GlobalVar.listMeta.Add(mtlmt);
                }
            }
        }

        public static void ClearWindow()
        {
            GlobalVar.listWidget.Clear();

            foreach (Widget item in GlobalVar.globalGrid)
            {
                GlobalVar.globalGrid.Remove(item);
            }

            GlobalVar.gridNumber = 1;
        }

        public static void Redraw()
        {
            UpdateWorkspace();

            GlobalVar.listWidget.Clear();

            foreach (Widget w in GlobalVar.globalGrid)
            {
                GlobalVar.globalGrid.Remove(w);
            }

            foreach (var item in GlobalVar.listMeta)
            {
                if (item.type == typeof(Entry))
                {
                    Entry entry = new Entry();
                    entry.Text = item.metastring;
                    GlobalVar.listWidget.Add(entry);
                }
                if (item.type == typeof(TextView))
                {
                    TextView textView = new TextView();
                    textView.Buffer.Text = item.metastring;
                    GlobalVar.listWidget.Add(textView);
                }
            }

            foreach (Widget item in GlobalVar.listWidget)
            {
                GlobalVar.globalGrid.Attach(Widgets.MovableWidget(item), 1, GlobalVar.gridNumber, 1, 1);
                GlobalVar.gridNumber++;
            }
        }
    }
}

