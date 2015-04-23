using System;
using Ast;
using Gtk;

namespace DesktopUI
{
    public class CasCalcView : Grid
    {
        public Entry input = new Entry();
        public Label output = new Label();
        Evaluator Eval;

        public CasCalcView(Evaluator Eval)
        {
            input.Activated += delegate
            {
                output.Text = Evaluate();
                ShowAll();
            };

            this.Eval = Eval;

            Attach(input, 1, 1, 1, 1);
            Attach(output, 1, 2, 1, 1);
            ShowAll();
        }

        string Evaluate()
        {
            return Eval.Evaluation(input.Text).ToString();
        }
    }
}