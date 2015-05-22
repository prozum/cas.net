using System;

namespace Ast
{
    public class WidgetData : EvalData
    {
        public string widget;

        public WidgetData(string widget)
        {
            this.widget = widget;
        }

        public override string ToString()
        {
            return widget;
        }
    }
}

