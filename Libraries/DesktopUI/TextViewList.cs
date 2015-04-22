using System;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
	public class TextViewList : Grid
	{
		List<MovableCasTextView> castextviews = new List<MovableCasTextView>();
		Button AddNewMovableTextView = new Button("+");

		public TextViewList() : base()
		{
			AddNewMovableTextView.Clicked += delegate
				{
					Insert("", false);
				};
		}

		public void Insert(string serializedString, bool teacherCanEdit)
		{
			castextviews.Add(new MovableCasTextView(serializedString, teacherCanEdit));

			Clear();

			foreach (Widget widget in castextviews)
			{
				Attach(widget, 1, Children.Length, 1, 1);
			}

			Attach(AddNewMovableTextView, 1, Children.Length, 1, 1);
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

