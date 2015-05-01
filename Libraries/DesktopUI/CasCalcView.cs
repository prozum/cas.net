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
            Eval.Parse(input.Text);
            EvalData res;

            output.Text = "";

            do
            {
                res = Eval.Step();
                DataList.Add(res);

            } while (!(res is DoneData));

            foreach (var data in DataList)
            {
                if (data is PrintData)
                {
                    output.Text += (data as PrintData).msg + "\n";
//                    buffer.Insert(ref insertIter, (data as PrintData).msg + "\n");
                }
                else if (data is ErrorData)
                {
                    output.Text += (data as ErrorData).err + "\n";
//                    buffer.InsertWithTagsByName(ref insertIter, (data as ErrorData).err + "\n", "error");
                }
                else if (data is DebugData)
                {
                    output.Text += (data as DebugData).expr.ToString() + "\n";
//                    buffer.Insert(ref insertIter, (data as ExprData).expr.ToString() + "\n");
                }   
            }

            DataList.Clear();
        }
    }
}