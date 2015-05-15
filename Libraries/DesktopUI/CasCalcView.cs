using System;
using System.Runtime;
using System.Collections.Generic;
using Ast;
using Gtk;

namespace DesktopUI
{
    public class CasCalcView : Grid
    {
        public Entry input = new Entry();
        public Label output = new Label();
        public Evaluator Eval;
        private List<EvalData> DataList = new List<EvalData>();

        public CasCalcView(Evaluator Eval)
        {
            this.Eval = Eval;

            Attach(input, 1, 1, 1, 1);
            Attach(output, 1, 2, 1, 1);
            ShowAll();
        }

        public void Evaluate()
        {
            Eval.Parse(input.Text);
            output.Text = Eval.Evaluate().ToString();
        }
    }
}