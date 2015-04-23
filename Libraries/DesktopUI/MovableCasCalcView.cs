using System;

namespace DesktopUI
{
	public class MovableCasCalcView : MovableCasTextView
	{
		CasCalcView calcview = new CasCalcView();

		public MovableCasCalcView(TextViewList parent) :
		base(parent, "", false)
		{
			Remove(textview);
			textview = null;
			Attach(calcview, 1, 1, 1, 2);
		}
	}
}

