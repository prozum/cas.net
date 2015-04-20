using System;
using Gtk;
using System.Collections.Generic;

namespace DesktopUI
{
    public class CasMovableWidget : Widget
    {
        Grid grid = new Grid();
        bool isStudentMovable = true;

        public CasMovableWidget(Widget widget, List<Widget> listWidget)
            : base()
        {
            Button buttonMoveUp = new Button("↑");
            buttonMoveUp.HeightRequest = 10;
            buttonMoveUp.WidthRequest = 10;
            buttonMoveUp.Clicked += delegate(object sender, EventArgs e)
            {
                int ID = listWidget.IndexOf(widget);
                if (ID >= 1)
                {
                    Widget w = listWidget[ID];
                    listWidget.RemoveAt(ID);
                    listWidget.Insert(ID - 1, w);
                }
            };

            Button buttonMoveDown = new Button("↓");
            buttonMoveDown.HeightRequest = 10;
            buttonMoveDown.WidthRequest = 10;
            buttonMoveDown.Clicked += delegate(object sender, EventArgs e)
            {
                int ID = listWidget.IndexOf(widget);
                if (ID <= listWidget.Count - 2)
                {
                    Widget w = listWidget[ID];
                    listWidget.RemoveAt(ID);
                    listWidget.Insert(ID + 1, w);
                }
            };

            grid.Attach(widget, 1, 1, 1, 2);
            grid.Attach(buttonMoveUp, 2, 1, 1, 1);
            grid.Attach(buttonMoveDown, 2, 2, 1, 1);

            ShowAll();
        }

        public Widget GetMovableWidget()
        {
            return grid;
        }

        public void SetStudentMovable(bool studentMovable)
        {
            isStudentMovable = studentMovable;
        }
    }
}

