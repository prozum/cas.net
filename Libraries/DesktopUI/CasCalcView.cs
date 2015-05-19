﻿using System;
using System.Runtime;
using System.Collections.Generic;
using Ast;
using Gtk;
using Draw;

namespace DesktopUI
{
    public class CasCalcView : Grid
    {
        public Entry input = new Entry();
        public Label output = new Label();
        public Evaluator eval;
        private List<EvalData> DataList = new List<EvalData>();
        public DrawView drawView;

        // Constructor for calcview
        public CasCalcView(Evaluator Eval)
        {
            this.eval = Eval;

            drawView = new DrawView();

            Attach(input, 1, 1, 1, 1);
            Attach(output, 1, 2, 1, 1);
            Attach(drawView, 1, 3, 1, 1);
            ShowAll();
        }

        // When run, the content of the input entry is evaluated, and the result returned to the output label
        public void Evaluate()
        {
            if(!string.IsNullOrEmpty(input.Text))
            {
                if(input.Text.Length == 0)
                {
                    output.Text = "No Input!";
                    return;
                }

                eval.Parse(input.Text);

                var res = eval.Evaluate();
                drawView.xList.Clear();
                drawView.xList.Clear();
                drawView.Hide();

                output.Text = string.Empty; // Clears output before adding new text

                if (!(res is Null || res is Error))
                {
                    output.Text = res.ToString() + "\n";
                }

                foreach(var data in eval.SideEffects)
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