using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using Ast;

namespace DesktopUI
{
    public class StudentGetFeedbackWindow : Window
    {
        User user;
        TextViewList textviews;

        public StudentGetFeedbackWindow(ref User user, ref TextViewList textviews)
            : base("Feedback")
        {
            throw new NotImplementedException();

            this.user = user;
            this.textviews = textviews;

            Label label = new Label("Name of the file you wish\nto get feedback on:");
            Entry entry = new Entry();

            Button buttonCancel = new Button("Cancel");
            buttonCancel.Clicked += delegate
            {
                Destroy();
            };

            Button buttonFeedback = new Button("Feedback");
            buttonFeedback.Clicked += delegate
            {
                string assignment = this.user.student.GetFeedback(entry.Text);

                List<MetaType> metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(assignment);

                this.textviews.castextviews.Clear();

                foreach (var metaItem in metaTypeList)
                {
                    if (metaItem.type == typeof(MovableCasCalcView))
                    {
                        Evaluator Eval = new Evaluator();
                        MovableCasCalcView movableCasCalcView = new MovableCasCalcView(Eval, this.textviews);
                        movableCasCalcView.calcview.input.Text = metaItem.metastring;
                        this.textviews.castextviews.Add(movableCasCalcView);
                    }
                    else if (metaItem.type == typeof(MovableCasTextView))
                    {
                        MovableCasTextView movableCasTextView = new MovableCasTextView(metaItem.metastring, true);
                        this.textviews.castextviews.Add(movableCasTextView);
                    }
                }

                this.textviews.castextviews.Reverse();

                this.textviews.Clear();
                this.textviews.Redraw();
                this.textviews.Reevaluate();
                this.textviews.ShowAll();

                Destroy();
            };

            Grid grid = new Grid();
            grid.Attach(label, 1, 1, 1, 1);
            grid.Attach(entry, 2, 1, 1, 1);
            grid.Attach(buttonCancel, 1, 2, 1, 1);
            grid.Attach(buttonFeedback, 2, 2, 1, 1);

            Add(grid);

            ShowAll();


        }
    }
}

