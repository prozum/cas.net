using System;
using Gtk;

namespace LibUI
{
    public static class Widgets
    {
        public static void AddEntryWidget()
        {
            Entry entry = (Entry)Widgets.EntryWidget();
            GlobalVar.listWidget.Add(entry);

            GlobalVar.globalGrid.Attach(MovableWidget(entry), 1, GlobalVar.gridNumber, 1, 1);
            GlobalVar.gridNumber++;
        }

        public static void AddTextViewWidget()
        {
            TextView textView = (TextView)Widgets.TextViewWidget();
            GlobalVar.listWidget.Add(textView);

            GlobalVar.globalGrid.Attach(MovableWidget(textView), 1, GlobalVar.gridNumber, 1, 1);
            GlobalVar.gridNumber++;
        }

        public static  TextView TextViewWidget()
        {
            TextView textView = new TextView();
            textView.HeightRequest = 100;
            textView.WidthRequest = 300;
            textView.Visible = true;

            return textView;
        }

        public static Widget EntryWidget()
        {
            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 300;
            entry.Buffer.Text = "";

            return entry;
        }

        public static Widget MovableWidget(Widget widget)
        {
            Grid grid = new Grid();

            Button buttonMoveUp = new Button("↑");
            buttonMoveUp.HeightRequest = 10;
            buttonMoveUp.WidthRequest = 10;
            buttonMoveUp.Clicked += delegate(object sender, EventArgs e)
            {
                int ID = GlobalVar.listWidget.IndexOf(widget);
                if (ID >= 1)
                {
                    Widget w = GlobalVar.listWidget[ID];
                    GlobalVar.listWidget.RemoveAt(ID);
                    GlobalVar.listWidget.Insert(ID - 1, w);
                    UpdateHandling.Redraw();
                }
            };

            Button buttonMoveDown = new Button("↓");
            buttonMoveDown.HeightRequest = 10;
            buttonMoveDown.WidthRequest = 10;
            buttonMoveDown.Clicked += delegate(object sender, EventArgs e)
            {
                int ID = GlobalVar.listWidget.IndexOf(widget);
                if (ID <= GlobalVar.listWidget.Count - 2)
                {
                    Widget w = GlobalVar.listWidget[ID];
                    GlobalVar.listWidget.RemoveAt(ID);
                    GlobalVar.listWidget.Insert(ID + 1, w);
                    UpdateHandling.Redraw();
                }
            };

            grid.Attach(widget, 1, 1, 1, 2);
            grid.Attach(buttonMoveUp, 2, 1, 1, 1);
            grid.Attach(buttonMoveDown, 2, 2, 1, 1);

            return grid;
        }
    }
}

