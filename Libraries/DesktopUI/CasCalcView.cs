using System;
using Gtk;
using System.Collections.Generic;

namespace DesktopUI
{
	public class CasCalcView : CasTextView
	{
		public CasCalcView(string SerializedString, bool TeacherCanEdit, List<Widget> widgets) :
		base(SerializedString, TeacherCanEdit, widgets)
		{

		}
	}
}

