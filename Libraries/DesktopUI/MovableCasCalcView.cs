using System;
using Ast;

namespace DesktopUI
{
	public class MovableCasCalcView : MovableCasTextView
	{
		CasCalcView calcview;

		public MovableCasCalcView(Evaluator Eval, TextViewList parent) :
		base(parent, "", false)
		{
			calcview = new CasCalcView (Eval);

			Remove(textview);
			textview = null;
			Attach(calcview, 1, 1, 1, 2);
		}
	}
}

