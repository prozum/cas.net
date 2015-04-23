using System;
using Ast;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
	public class TextViewList : Grid
	{
		List<MovableCasTextView> castextviews = new List<MovableCasTextView>();

		public List<MovableCasTextView> TextViews
		{
			get
			{ return castextviews; }
		}

		Grid ButtonGrid = new Grid();
		Evaluator Eval = new Evaluator ();
		Button AddNewMovableTextView = new Button("New Textbox");
		Button AddNewMovableCalcView = new Button("New Calcbox");

		public TextViewList() : base()
		{
			AddNewMovableTextView.Clicked += delegate
				{
					InsertTextView("", false);
				};

			AddNewMovableCalcView.Clicked += delegate
				{
					InsertCalcView();
				};

			ButtonGrid.Attach(AddNewMovableTextView, 1, 1, 1, 1);
			ButtonGrid.Attach(AddNewMovableCalcView, 2, 1, 1, 1);

			Attach(ButtonGrid, 1, 1, 1, 1);

			ShowAll();
		}

		public void InsertTextView(string serializedString, bool teacherCanEdit)
		{
			castextviews.Add(new MovableCasTextView(this, serializedString, teacherCanEdit));

			Clear();
			Redraw();
			ShowAll();
		}

		public void InsertCalcView()
		{
			castextviews.Add(new MovableCasCalcView(Eval, this));

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

			Attach(ButtonGrid, 1, Children.Length, 1, 1);
		}
	}
}

