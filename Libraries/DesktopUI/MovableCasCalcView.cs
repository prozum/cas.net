using System;
using Ast;

namespace DesktopUI
{
    // Used for creating the movable version of movablecascalcview
    public class MovableCasCalcView : MovableCasTextView
    {
        public CasCalcView calcview;

        public MovableCasCalcView(Evaluator Eval)
            : base("", false)
        {
            calcview = new CasCalcView(Eval);

            Remove(textview);
            Attach(calcview, 1, 1, 1, 2);
        }
    }
}