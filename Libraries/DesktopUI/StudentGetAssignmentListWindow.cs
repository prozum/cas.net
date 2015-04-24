using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using Ast;

namespace DesktopUI
{
    public class StudentGetAssignmentListWindow : Window
    {
        User user;
        TextViewList textviews;

        public StudentGetAssignmentListWindow(ref User user, ref TextViewList textviews)
            : base("Get Assignment List")
        {
            this.user = user;
            this.textviews = textviews;

            string[] assignmentList = user.student.GetAssignmentList();

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            VBox vbox = new VBox(false, 2);

            foreach (var item in assignmentList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Button button = new Button(item);
                    button.Clicked += delegate
                    {
                        string assignment = this.user.student.GetAssignment(item);

                        List<MetaType> metaTypeList = new List<MetaType>();

                        metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(assignment);

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
                                MovableCasTextView movableCasTextView = new MovableCasTextView(this.textviews, metaItem.metastring, true);
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
                    vbox.Add(button);
                }
            }

            scrolledWindow.Add(vbox);
            Add(scrolledWindow);

            SetDefaultSize(300, 200);

            ShowAll();

        }
    }
}

