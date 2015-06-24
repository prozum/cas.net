using System;
using Gtk;
using Ast;

namespace Widget
{
    public class WidgetView : Box
    {
        public event EventHandler Changed;

        public WidgetView () : base(Orientation.Vertical, 0)
        {
        }

        public void AddWidget(WidgetData data)
        {
            Widget widget;

            if (data is CheckboxData)
            {
                widget = new Checkbox(data as CheckboxData);
            }
            else
                throw new Exception("Unhandled widget: " + data.GetType().Name);
                
            Add(widget);
        }

        public void Invoke()
        {
            Changed.Invoke(this, null);
        }
    }
}

