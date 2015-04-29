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
        //        public Label output = new Label();
        public TextView output = new TextView();
        TextBuffer buffer;
        public Evaluator Eval;
        private List<EvalData> DataList = new List<EvalData>();

        public CasCalcView(Evaluator Eval)
        {
            this.Eval = Eval;
            buffer = output.Buffer;

            Attach(input, 1, 1, 1, 1);
            Attach(output, 1, 2, 1, 1);
            ShowAll();
        }

        public void Evaluate()
        {
            TextIter insertIter = buffer.StartIter;

            Eval.Parse(input.Text);
            EvalData res;

            do
            {
                res = Eval.Step();
                DataList.Add(res);

            } while (!(res is DoneData));

            foreach (var data in DataList)
            {
                if (data is PrintData)
                {
                    buffer.Insert(ref insertIter, (data as PrintData).msg + "\n");
                }
                else if (data is ErrorData)
                {
                    buffer.InsertWithTagsByName(ref insertIter, (data as ErrorData).msg + "\n", "error");
                }
                else if (data is ExprData)
                {
                    buffer.Insert(ref insertIter, (data as ExprData).expr.ToString() + "\n");
                }   
            }

            DataList.Clear();
        }
    }
}