using System;
using Ast;

namespace DesktopUI
{
    public class MovableCasCalcView : MovableCasTextView
    {
        public CasCalcView calcview;

        public MovableCasCalcView(Evaluator Eval)
            : base("", false)
        {
            calcview = new CasCalcView(Eval);

            Remove(textview);
            textview = null;
            Attach(calcview, 1, 1, 1, 2);
        }
    }
}