using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
	public class TextViewList : Grid
	{
		List<MovableCasTextView> castextviews = new List<MovableCasTextView>();

		public TextViewList() : base()
		{
			
		}

		public void Insert(string serializedString, bool teacherCanEdit)
		{
			Attach(new MovableCasTextView(serializedString, teacherCanEdit), 1, Children.Length, 1, 1);
			ShowAll();
		}

		public void Clear()
		{
			foreach (Widget widget in this)
			{
				Remove(widget);
			}

			ShowAll();
		}
	}
}

