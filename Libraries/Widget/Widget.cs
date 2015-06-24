using System;
using Ast;
using Gtk;


namespace Widget
{
    public class Widget : Box
    {
        public Text Text;
        public Variable Variable;

        public Widget(Text text, Variable @var) : base(Orientation.Horizontal, 0)
        {
            Text = text;
            Variable = @var;

            Add(new Label(Text.ToString() + " (" + Variable.Identifier + ")"));
        }
    }
}

