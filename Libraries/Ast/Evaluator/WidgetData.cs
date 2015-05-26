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

    public class RelativeData : LayoutData
    {
        public int Spacing;
    }

    public class GridData : LayoutData
    {
        public int Spacing;
    }

    public class VBoxData : LayoutData
    {

    }

    public class HBoxData : LayoutData
    {

    }

    public class LabelData : WidgetData
    {
        public string Text;

        public LabelData(string text) : base("Label")
        {
            Text = text;
        }
    }

    public class TextViewData : WidgetData
    {
        public string Text;

        public TextViewData(string text) : base("TextView")
        {
            Text = text;
        }
    }

    public class EntryData : WidgetData
    {
        public EntryData(int x, int y) : base("Entry")
        {

        }
    }

    public class ButtonData : WidgetData
    {
        public ButtonData(int x, int y) : base("Button")
        {

        }
    }

    public class ImageData : WidgetData
    {
        public ImageData(int x, int y) : base("Image")
        {

        }
    }

    public class ListViewData : WidgetData
    {
        public ListViewData(int x, int y) : base("ListView")
        {

        }
    }
}

