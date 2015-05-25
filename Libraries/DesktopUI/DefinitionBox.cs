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

        // Constructor for the definition box, used to show declared variables.
		public DefinitionBox (Evaluator Eval) : base()
		{
            renderer = new CellRendererText();
            AppendColumn("Variable", renderer, "text", 0);

            renderer = new CellRendererText();
            AppendColumn("Value", renderer, "text", 1);

            list = new ListStore(typeof(string), typeof(string));
            Model = list;

            this.Eval = Eval;
            this.Eval.Locals.Clear();
		}

        // Updates all elements on update
		public void Update()
		{
            list.Clear();

            foreach (var def in Eval.Locals)
            {
                if(def.Value is SysFunc)
                {
                    list.AppendValues(def.Value.ToString(), "System Magic");
                }
                else if(def.Value is VarFunc)
                {
                    list.AppendValues(def.Value.ToString(), (def.Value as VarFunc).Definition.ToString());
                }
                else if(def.Value is Scope)
                {
                    list.AppendValues(def.Key, def.Value.ToString());
                }
                else
                {
                    list.AppendValues(def.Key, def.Value.Value.ToString());
                }

                //list.AppendValues(def.Value.ToString(), def.Value.Value.ToString());
            }
  
			ShowAll ();
		}
	}
}