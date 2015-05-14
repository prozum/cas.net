using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using Ast;

namespace DesktopUI
{
    class MovableLockedCasCalcView : MovableCasCalcView
    {
        readonly CheckButton checkButton = new CheckButton("Lock for students");

        public MovableLockedCasCalcView(Evaluator Eval, bool locked)
            : base(Eval, locked)
        {

        }
    }
}
