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

			Attach(AddNewMovableTextView, 1, 1, 1, 1);
		}

		public void Insert(string serializedString, bool teacherCanEdit)
		{
			castextviews.Add(new MovableCasTextView(this, serializedString, teacherCanEdit));

			Clear();
			Redraw();
			ShowAll();
		}

		public void Clear()
		{
			foreach (Widget widget in this)
			{
				Remove(widget);
			}
		}

		public void Move(int ID, int UpOrDown)
		{
			int i = 0;

			while (ID != castextviews[i].id_)
			{
				i++;
			}

			// move down
			if (UpOrDown == 1 && i+1 < castextviews.Count)
			{
				castextviews.Insert(i+2, castextviews[i]);
				castextviews.RemoveAt(i);
			}
			// move up
			else if (UpOrDown == -1 && i-1 >= 0)
			{
				castextviews.Insert(i-1, castextviews[i]);
				castextviews.RemoveAt(i+1);
			}

			Clear();
			Redraw();
		}

		public void Redraw()
		{
			foreach (Widget widget in castextviews)
			{
				Attach(widget, 1, Children.Length, 1, 1);
			}

			Attach(AddNewMovableTextView, 1, Children.Length, 1, 1);
		}
	}
}

