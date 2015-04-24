using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    public class StudentGetAssignmentMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public StudentGetAssignmentMenuItem(ref User user, ref TextViewList textviews)
            : base("Get Assignment")
        {
            this.user = user;
            this.textviews = textviews;
            this.Activated += delegate
            {
                OnClicked();
            };
        }

        void OnClicked()
        {
            Window window = new Window("Get Assignment");

            window.SetSizeRequest(300, 300);

            Entry entry = new Entry();
            Label label = new Label("Assignment:");

            Button buttonCancel = new Button("Cancel");
            buttonCancel.Clicked += delegate
            {
                window.Destroy();
            };

            Button buttonAssignment = new Button("Get Assignment");
            buttonAssignment.Clicked += delegate
            {
                string assignment = user.student.GetAssignment(entry.Text);

                List<MetaType> metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(assignment);

                textviews.castextviews.Clear();

                foreach (var metaItem in metaTypeList)
                {
                    if (metaItem.type == typeof(MovableCasCalcView))
                    {
                        Evaluator Eval = new Evaluator();
                        MovableCasCalcView movableCasCalcView = new MovableCasCalcView(Eval, textviews);
                        movableCasCalcView.calcview.input.Text = metaItem.metastring;
                        textviews.castextviews.Add(movableCasCalcView);
                    }
                    else if (metaItem.type == typeof(MovableCasTextView))
                    {
                        MovableCasTextView movableCasTextView = new MovableCasTextView(textviews, metaItem.metastring, true);
                        textviews.castextviews.Add(movableCasTextView);
                    }
                }

                textviews.castextviews.Reverse();

                textviews.Clear();
                textviews.Redraw();
                textviews.Reevaluate();
                textviews.ShowAll();

                window.Destroy();
            };

            Grid grid = new Grid();
            grid.Attach(label, 1, 1, 1, 1);
            grid.Attach(entry, 2, 1, 1, 1);
            grid.Attach(buttonCancel, 1, 2, 1, 1);
            grid.Attach(buttonAssignment, 2, 2, 1, 1);

            window.Add(grid);

            window.ShowAll();
        }
    }
}

