using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ast;

namespace DesktopUI
{
    // The movable version of a multiline view
    public class MovableCasCalcMulitlineView : MovableCasTextView
    {
        public CasCalcMultilineView calcview;

        public MovableCasCalcMulitlineView(string serializedMultiline, Evaluator eval) : base("",false)
        {
            calcview = new CasCalcMultilineView(serializedMultiline, eval);

            Remove(textview);
            Attach(calcview, 1, 1, 1, 2);
        }
    }
}
