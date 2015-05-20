using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ast;

namespace DesktopUI
{
    public class MovableCasCalcMulitlineView : MovableCasTextView
    {
        public CasCalcMultilineView calcview;

        public MovableCasCalcMulitlineView(Evaluator eval) : base("",false)
        {
            calcview = new CasCalcMultilineView(eval);

            Remove(textview);
            Attach(calcview, 1, 1, 1, 2);
        }
    }
}
