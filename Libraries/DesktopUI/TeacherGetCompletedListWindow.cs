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
        readonly TextViewList textviews;
        string Filename;
        string[] StudentList;

        public TeacherGetCompletedListWindow(ref User user, ref TextViewList textviews, string Filename)
            : base("Get List of Completed Students")
        {
            this.user = user;
            this.textviews = textviews;
            this.Filename = Filename;

            SetSizeRequest(300, 300);

            Grid grid = new Grid();

            Label GradeLabel = new Label("Grade:");
            Entry GradeEntry = new Entry();
            GradeEntry.WidthRequest = 200;

            Button CancelButton = new Button("Cancel");
            CancelButton.Clicked += delegate
            {
                Destroy();
            };

            Button DownloadButton = new Button("List of Completed Students");
            DownloadButton.Clicked += (sender, e) =>
            {
                StudentList = this.user.teacher.GetCompletedList(this.Filename, GradeEntry.Text);
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
                        string completed = this.user.teacher.GetCompleted(button.TooltipText, this.Filename, GradeEntry.Text);
                        LoadWorkspace(completed);
                    };
                    grid.Attach(button, 1, 1 + i, 1, 1);
                    grid.Attach(label, 2, 1 + i, 1, 1);
                    ShowAll();
                }
            };
             
            grid.Attach(GradeLabel, 2, 1, 1, 1);
            grid.Attach(GradeEntry, 2, 2, 1, 1);

            grid.Attach(DownloadButton, 1, 3, 1, 1);
            grid.Attach(CancelButton, 2, 3, 1, 1);

            Add(grid);
            ShowAll();
        }

        void LoadWorkspace(string completed)
        {
            List<MetaType> metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(completed);

            this.textviews.castextviews.Clear();

            foreach (var metaItem in metaTypeList)
            {
                if (metaItem.type == typeof(MovableCasCalcView))
                {
                    textviews.InsertCalcView(metaItem.metastring);
                }
                else if (metaItem.type == typeof(MovableCasTextView))
                {
                    textviews.InsertTextView(metaItem.metastring, metaItem.locked, -1);
                }
            }

            this.textviews.castextviews.Reverse();

            this.textviews.Clear();
            this.textviews.Redraw();
            this.textviews.Reevaluate();
            this.textviews.ShowAll();

            Destroy();

        }
    }
}