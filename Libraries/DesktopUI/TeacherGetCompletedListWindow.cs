using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    public class TeacherGetCompletedListWindow : Window
    {
        User user;
        TextViewList textviews;
        string[] StudentList;

        public TeacherGetCompletedListWindow(ref User user, ref TextViewList textviews)
            : base("Get List of Completed Students")
        {
            this.user = user;
            this.textviews = textviews;

            SetSizeRequest(300, 300);

            Grid grid = new Grid();

            Label FileNameLabel = new Label("Filename:");
            Entry FileNameEntry = new Entry();
            FileNameEntry.WidthRequest = 200;

            Label GradeLabel = new Label("Grade:");
            Entry GradeEntry = new Entry();
            GradeEntry.WidthRequest = 200;

            Button CancelButton = new Button("Cancel");
            CancelButton.Clicked += delegate
            {
                Destroy();
            };

            Button DownloadButton = new Button("List of Completed Students");
            DownloadButton.Clicked += delegate
            {
                StudentList = this.user.teacher.GetCompletedList(FileNameEntry.Text, GradeEntry.Text);

                foreach (Widget widget in grid)
                {
                    grid.Remove(widget);
                }

                for (int i = 0; i < StudentList.Length / 2; i++)
                {
                    Button button = new Button(StudentList[2 * i]);
                    button.TooltipText = StudentList[2 * i];
                    button.HasTooltip = false;
                    Label label = new Label(StudentList[(2 * i) + 1]);
					
                    button.Clicked += delegate
                    {
                        string completed = this.user.teacher.GetCompleted(button.TooltipText, FileNameEntry.Text, GradeEntry.Text);

                        Console.WriteLine(completed);

                        List<MetaType> metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(completed);

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

                    grid.Attach(button, 1, 1 + i, 1, 1);
                    grid.Attach(label, 2, 1 + i, 1, 1);
					
                    ShowAll();				
                }
            };

            grid.Attach(FileNameLabel, 1, 1, 1, 1);
            grid.Attach(FileNameEntry, 1, 2, 1, 1);

            grid.Attach(GradeLabel, 2, 1, 1, 1);
            grid.Attach(GradeEntry, 2, 2, 1, 1);

            grid.Attach(DownloadButton, 1, 3, 1, 1);
            grid.Attach(CancelButton, 2, 3, 1, 1);

            Add(grid);
            ShowAll();
        }
    }
}