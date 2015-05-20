using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ast;
using Gtk;
using Draw;

namespace DesktopUI
{
    public class CasCalcMultilineView : Grid
    {
        public Evaluator eval;
        public TextView input = new TextView();
        public Button evaluateButton = new Button("Evaluate");
        private List<EvalData> DataList = new List<EvalData>();
        public Label output = new Label();
        public DrawView drawView = new DrawView();

        public CasCalcMultilineView(Evaluator eval) : base()
        {
            this.eval = eval;

            Attach(input, 1, 1, 1, 1);
            Attach(evaluateButton, 1, 2, 1, 1);
            Attach(output, 1, 3, 1, 1);
            Attach(drawView, 1, 4, 1, 1);
            ShowAll();
        }

        public void Evaluate()
        {
            if (!string.IsNullOrEmpty(input.Buffer.Text))
            {
                if (input.Buffer.Text.Length == 0)
                {
                    output.Text = "No Input!";
                    return;
                }

                eval.Parse(input.Buffer.Text);

                var res = eval.Evaluate();
                drawView.xList.Clear();
                drawView.xList.Clear();
                drawView.Hide();

                output.Text = string.Empty; // Clears output before adding new text

                if (!(res is Null || res is Error))
                {
                    output.Text = res.ToString() + "\n";
                }

                foreach (var data in eval.SideEffects)
                {
                    Console.WriteLine("Sideeffect: " + data.ToString());
                }

                foreach (var data in eval.SideEffects)
                {

                    if (data is PrintData)
                    {
                        output.Text += data.ToString() + "\n";
                    }
                    else if (data is ErrorData)
                    {
                        output.Text += data.ToString() + "\n";
                    }
                    else if (data is DebugData && eval.GetBool("debug"))
                    {
                        output.Text += data.ToString() + "\n";
                    }
                    else if (data is PlotData)
                    {
                        drawView.Plot(data as PlotData);
                    }
                }
            }
        }
    }
}
