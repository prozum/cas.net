using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    public class StudentGetAssignmentListMenuItem : MenuItem
    {
        TextViewList textviews;
        User user;
        string file;

        public StudentGetAssignmentListMenuItem(ref User user, ref TextViewList textviews)
            : base("Get List of Assignments")
        {
            this.textviews = textviews;
            this.user = user;
            this.Activated += delegate
            {
                Onclicked();
            };
        }

        void Onclicked()
        {
//            throw new NotImplementedException();

            Window window = new Window("Get Assignment List");
          
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
                        file = user.student.GetAssignment(item);

                        List<MetaType> metaTypeList = new List<MetaType>();

                        metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(file);

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
                    vbox.Add(button);
                }
            }

            scrolledWindow.Add(vbox);
            window.Add(scrolledWindow);

            window.SetDefaultSize(300, 200);

            window.ShowAll();
        }
    }
}

