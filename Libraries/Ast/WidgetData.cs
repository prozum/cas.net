using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class WidgetData : EvalData
    {
        public string widget;
        bool show;

        static int ID = 0;
        private int _id;
        public int id
        {
            get
            {
                return _id;
            }
        }

        public WidgetData(string widget)
        {
            this.widget = widget;
            show = false;
            _id = ID++;
        }

        public override string ToString()
        {
            return widget;
        }

        public void Show()
        {
            show = true;
        }

        public void Hide()
        {
            show = false;
        }
    }

    public class WindowData : WidgetData
    {
        LayoutData Layout;

        public WindowData(LayoutData layout) : base("Window")
        {
            Layout = layout;
        }
    }

    public class LayoutData : WidgetData
    {
        List<WidgetData> ChildrenList = new List<WidgetData>();

        public int Children;

        public LayoutData() : base("Layout")
        {
            Children = 0;
        }

        // add new widget to window/activity
        public void Add(WidgetData widget)
        {
            ChildrenList.Add(widget);
            Children++;
        }

        // return true if child was found, else return false
        public bool Remove(int id)
        {
            int pos = ChildrenList.FindIndex(x => x.id == id);

            if (pos != -1)
            {
                ChildrenList.RemoveAt(pos);
                Children--;
                return true;
            }

            return false;
        }

        // clear the window/activity
        public void Clear()
        {
            ChildrenList.Clear();
            Children = 0;
        }
    }

    public class Relative : LayoutData
    {
        public int Spacing;
    }

    public class Grid : LayoutData
    {
        public int Spacing;
    }

    public class VBox : LayoutData
    {

    }

    public class HBox : LayoutData
    {

    }

    public class Label : WidgetData
    {
        public string Text;

        public Label(string text) : base("Label")
        {
            Text = text;
        }
    }

    public class TextView : WidgetData
    {
        public string Text;

        public TextView(string text) : base("TextView")
        {
            Text = text;
        }
    }

    public class Entry : WidgetData
    {
        public Entry(int x, int y) : base("Entry")
        {

        }
    }

    public class Button : WidgetData
    {
        public Button(int x, int y) : base("Button")
        {

        }
    }

    public class Image : WidgetData
    {
        public Image(int x, int y) : base("Image")
        {

        }
    }

    public class ListView : WidgetData
    {
        public ListView(int x, int y) : base("ListView")
        {

        }
    }
}

