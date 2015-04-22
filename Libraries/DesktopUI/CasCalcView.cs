using System;
using Ast;
using Gtk;

namespace DesktopUI
{
	public class CasCalcView : CasTextView
	{
		public CasCalcView(TextViewList parent, string serializedString, bool teacherCanEdit) :
		base(serializedString, teacherCanEdit)
		{

		}

		public void Evaluate()
		{
			string expression = String.Empty;
			string buffer = Buffer.ToString();
			int i = 0;

			while (buffer[i] != '\n')
			{
				expression += buffer[i];
			}

			Evaluator Eval = new Evaluator();
			EvalData Data = Eval.Evaluation(expression);

			Buffer.Clear();
			TextIter textIter = Buffer.StartIter;
			byte[] EvaluationBytes = Convert.FromBase64String(expression += '\n' + Data.ToString());
			Buffer.Deserialize(Buffer, Buffer.RegisterDeserializeTagset(null), ref textIter, EvaluationBytes, (ulong)EvaluationBytes.Length);
		}
	}
}