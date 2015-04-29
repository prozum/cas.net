using System;
using System.Runtime;
using System.Collections.Generic;
using Ast;
using Gtk;

namespace DesktopUI
{
    public class CasCalcView : Grid
    {
        public Entry input = new Entry();
        public Label output = new Label();
        public Evaluator Eval;
		private List<EvalData> DataList = new List<EvalData>();

        public CasCalcView(Evaluator Eval)
        {
            this.Eval = Eval;

            Attach(input, 1, 1, 1, 1);
            Attach(output, 1, 2, 1, 1);
            ShowAll();
        }

        public void Evaluate()
        {
			Eval.Parse(input);
			output.Text = String.Empty;
			string res = String.Empty;

			do
			{
				res = Eval.Step();
				DataList.Add(res);

			} while (!(res is DoneData));

			foreach (var data in DataList)
			{
				if (data is MsgData)
				{
					switch ((data as MsgData).type)
					{
						case MsgType.Print:
							output.Text += (data as MsgData).msg + "\n";
							break;
						case MsgType.Error:
							output.Text += (data as MsgData).msg + "error" + "\n";
							break;
						case MsgType.Info:
							output.Text += (data as MsgData).msg + "info" + "\n";
					}
				}
				else if (data is ExprData)
				{
					output.Text += (data as ExprData).expr.ToString() + "\n";
				}
			}

			DataList.Clear();
        }
    }
}