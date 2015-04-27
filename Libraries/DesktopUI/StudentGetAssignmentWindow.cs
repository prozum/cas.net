using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    public class StudentGetAssignmentWindow : Window
    {
        User user;
        TextViewList textviews;

        public StudentGetAssignmentWindow(ref User user, ref TextViewList textviews)
            : base("Get Assignment")
        {
            this.user = user;
            this.textviews = textviews;

            SetSizeRequest(300, 300);

            Entry entry = new Entry();
            Label label = new Label("Assignment:");

            Button buttonCancel = new Button("Cancel");
            buttonCancel.Clicked += delegate
            {
                Destroy();
            };

            Button buttonAssignment = new Button("Get Assignment");
            buttonAssignment.Clicked += delegate
            {
                string assignment = this.user.student.GetAssignment(entry.Text);

                List<MetaType> metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(assignment);

                this.textviews.castextviews.Clear();

                foreach (var metaItem in metaTypeList)
                {
                    if (metaItem.type == typeof(MovableCasCalcView))
                    {
                        Evaluator Eval = new Evaluator();
                        MovableCasCalcView movableCasCalcView = new MovableCasCalcView(Eval);
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
            grid.Attach(buttonAssignment, 2, 2, 1, 1);

            Add(grid);

            ShowAll();

        }
    }
}

