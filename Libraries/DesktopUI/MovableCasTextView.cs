using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
	public class MovableCasTextView : Grid
	{
		CasTextView textview;
		Button ButtonMoveUp = new Button("↑");
		Button ButtonMoveDown = new Button("↓");
		/* insert arror moving thingy here */

		public MovableCasTextView(string serializedString, bool teacherCanEdit)
		{
			textview = new CasTextView(serializedString, teacherCanEdit);

			Attach(textview, 1, 1, 1, 2);
			Attach(ButtonMoveUp, 2, 1, 1, 1);
			Attach(ButtonMoveDown, 2, 2, 1, 1);
		}
	}
}

