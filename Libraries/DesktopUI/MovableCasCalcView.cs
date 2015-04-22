using System;

namespace DesktopUI
{
	public class MovableCasCalcView : MovableCasTextView
	{
		public MovableCasCalcView(TextViewList parent, string serializedString, bool teacherCanEdit) :
		base(parent, serializedString, teacherCanEdit)
		{
			textview = new CasCalcView(serializedString, teacherCanEdit);
			textview.WidthRequest = 400;
			textview.HeightRequest = 200;

			Attach(textview, 1, 1, 1, 2);
		}
	}
}

