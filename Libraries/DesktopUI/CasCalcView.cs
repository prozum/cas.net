using System;
using System.Text;
using Gtk;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
	public class CasCalcView : CasTextView
	{
		string calculation;
		Evaluator evaluator;

		public CasCalcView(Evaluator evaluator, string SerializedString, bool TeacherCanEdit, List<Widget> widgets) :
		base(SerializedString, TeacherCanEdit, widgets)
		{
			this.evaluator = evaluator;
		}

		void Calculate()
		{
			calculation = Buffer.ToString();
			EvalData result = evaluator.Evaluation(calculation);

			calculation += "\n" + result.ToString();
			Buffer.Clear();
			Buffer.Text.Insert();
		}
	}
}

