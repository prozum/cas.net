using System;
using Ast;
using Gtk;

namespace DesktopUI
{
	public class DefinitionBox : TreeView
	{
        CellRenderer renderer;
        ListStore list;

        readonly Evaluator Eval;

		public DefinitionBox (Evaluator Eval) : base()
		{
            renderer = new CellRendererText();
            AppendColumn("Variable", renderer, "text", 0);

            renderer = new CellRendererText();
            AppendColumn("Value", renderer, "text", 1);

            list = new ListStore(typeof(string), typeof(string));
            Model = list;

            this.Eval = Eval;
            this.Eval.locals.Clear();
		}

		public void Update()
		{
            list.Clear();

            foreach (var def in Eval.locals)
            {
                list.AppendValues(def.Key, def.Value.ToString());
            }
  
			ShowAll ();
		}
	}
}