using Ast;
using System;
using Gtk;

namespace Widget
{
    public class Checkbox : Widget
    {
        CheckButton Input = new CheckButton();

        public Checkbox(CheckboxData data) : base(data.Text, data.Variable)
        {
            Add(Input);

            Input.Toggled += (sender, e) => 
                { 
                    Variable.Value = new Ast.Boolean(Input.Active);
                    (Parent as WidgetView).Invoke();
                };
        }
    }
}

