using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
	public class TextViewList : Widget
	{
		Grid grid = new Grid();
		List<TextView> castextviews = new List<TextView>();

		public TextViewList() : base()
		{
		}

		public Widget GetMovableWidget()
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}
	}
}

