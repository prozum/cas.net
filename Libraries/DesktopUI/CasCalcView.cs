using System;
using Ast;
using Gtk;

namespace DesktopUI
{
	public class CasCalcView : Grid
	{
		Entry input = new Entry();
		public Label output = new Label();

		public CasCalcView()
		{
			input.Activated += delegate
				{
					output.Text = Evaluate();
					ShowAll();
				};

			Attach(input, 1, 1, 1, 1);
			Attach(output, 1, 2, 1, 1);
			ShowAll();
		}

		string Evaluate()
		{
			Evaluator Eval = new Evaluator();

			return Eval.Evaluation(input.Text).ToString();
		}
	}
}