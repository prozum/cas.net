using System;
using Ast;
using Gtk;

namespace DesktopUI
{
	public class DefinitionBox : Grid
	{
        CellRenderer renderer;

        TreeStore DefinitionStore;
        TreeView DefinitionTree;

        readonly Evaluator Eval;

        // Constructor for the definition box, used to show declared variables.
        public DefinitionBox(Evaluator Eval)
            : base()
		{
            DefinitionStore = new TreeStore(typeof(string), typeof(string));
            DefinitionTree = new TreeView(DefinitionStore);
            Expand = true;

            renderer = new CellRendererText();
            DefinitionTree.AppendColumn("Variable", renderer, "text", 0);
            renderer = new CellRendererText();
            DefinitionTree.AppendColumn("Value", renderer, "text", 1);

            this.Eval = Eval;

            Add(DefinitionTree);

            UpdateDefinitions();
		}

		public void UpdateDefinitions()
		{
            TreeIter iter;
            DefinitionStore.Clear();

            foreach (var def in Eval.Locals)
            {
                if(def.Value is SysFunc)
                {
                    iter = DefinitionStore.AppendValues(def.Value.ToString(), "System Magic");
                    UpdateScope(def.Value as Scope, iter);
                }
                else if(def.Value is VarFunc)
                {
                    iter = DefinitionStore.AppendValues(def.Value.ToString(), (def.Value as VarFunc).Definition.ToString());
                    UpdateScope(def.Value as Scope, iter);
                }
                else if(def.Value is Scope)
                {
                    iter = DefinitionStore.AppendValues(def.Key, def.Value.ToString());
                    UpdateScope(def.Value as Scope, iter);
                }
                else
                {
                    DefinitionStore.AppendValues(def.Key, def.Value.Value.ToString());
                }
            }
            QueueDraw();
			ShowAll ();
		}

        public void UpdateScope(Scope scope, TreeIter lastIter)
        {
            TreeIter iter;

            foreach (var def in scope.Locals)
            {
                if (def.Value is SysFunc)
                {
                    iter = DefinitionStore.AppendValues(lastIter, def.Value.ToString(), "System Magic");
                    UpdateScope(def.Value as Scope, iter);
                }
                else if (def.Value is VarFunc)
                {
                    iter = DefinitionStore.AppendValues(lastIter, def.Value.ToString(), (def.Value as VarFunc).Definition.ToString());
                    UpdateScope(def.Value as Scope, iter);
                }
                else if (def.Value is Scope)
                {
                    iter = DefinitionStore.AppendValues(lastIter, def.Key.ToString(), def.Value.ToString());
                    UpdateScope(def.Value as Scope, iter);
                }
                else
                {
                    DefinitionStore.AppendValues(lastIter, def.Key, def.Value.ToString());
                }
            }
        }
	}
}