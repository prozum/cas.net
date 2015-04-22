using System;

namespace DesktopUI
{
	public class MovableCasCalcView : MovableCasTextView
	{
		public MovableCasCalcView(TextViewList parent) :
		base(parent, "", false)
		{
			textview = (CasCalcView)new CasCalcView();
			textview.WidthRequest = 400;
			textview.HeightRequest = 200;

			Attach(textview, 1, 1, 1, 2);
		}
	}
}

