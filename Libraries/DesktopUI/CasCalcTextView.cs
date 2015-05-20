using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ast;
using Gtk;

namespace DesktopUI
{
    class CasCalcTextView : CasCalcView
    {
        Evaluator eval;
        public TextView calcTextView = new TextView();
        public Button evaluateButton = new Button("Evaluate");

        public CasCalcTextView(Evaluator eval) : base(eval)
        {
            this.eval = eval;

            Remove(input);

            Attach(calcTextView, 1, 1, 1, 1);
            Attach(evaluateButton, 1, 2, 1, 1);
            ShowAll();
        }
    }
}
