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
        public CasTextView input = new CasTextView("",false);
        public Button evaluateButton = new Button("Evaluate");
        public Label output = new Label();
        public DrawView drawView = new DrawView();

        public CasCalcMultilineView(String serializedMultiview, Evaluator eval) : base()
        {
            this.eval = eval;

            input.WrapMode = WrapMode.WordChar;
            DeserializeCasTextView(serializedMultiview);

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

                string outputstring = String.Empty;

                output.Text = string.Empty; // Clears output before adding new text

                if (!(res is Null || res is Error))
                {
                    outputstring = res.ToString() + "\n";
                }

                foreach (var data in eval.SideEffects)
                {

                    if (data is PrintData)
                    {
                        outputstring += data.ToString() + "\n";
                    }
                    else if (data is ErrorData)
                    {
                        outputstring += data.ToString() + "\n";
                    }
                    else if (data is DebugData && eval.GetBool("debug"))
                    {
                        outputstring += data.ToString() + "\n";
                    }
                    else if (data is PlotData)
                    {
                        drawView.Plot(data as PlotData);
                    }
                }

                output.Text = outputstring;
            }
        }

        public string SerializeCasTextView()
        {
            TextIter startIter, endIter;
            input.Buffer.GetBounds(out startIter, out endIter);
            byte[] byteTextView = input.Buffer.Serialize(input.Buffer, input.Buffer.RegisterSerializeTagset(null), startIter, endIter);
            string serializedTextView = Convert.ToBase64String(byteTextView);

            return serializedTextView;
        }

        // A method for deserializing the content serialized in the above method.
        public void DeserializeCasTextView(string serializedTextView)
        {
            TextIter textIter = input.Buffer.StartIter;
            byte[] byteTextView = Convert.FromBase64String(serializedTextView);
            input.Buffer.Deserialize(input.Buffer, input.Buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);
        }

    }
}
