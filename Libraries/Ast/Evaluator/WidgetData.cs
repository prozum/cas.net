using System.Collections.Generic;

namespace Ast
{
    public abstract class WidgetData : EvalData
    {
        public Text Text;
        public Variable Variable;

        protected WidgetData(Text text, Variable variable)
        {
            Text = text;
            Variable = variable;
        }
    }
}

