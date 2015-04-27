using System;
using Gtk;
using ImEx;
using System.Collections.Generic;

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

					Console.WriteLine (StudentList.Length);

                for (int i = 0; i < StudentList.Length; i++)
                {
                    Button button = new Button(StudentList[i]);

                    button.Clicked += delegate
                    {
                        List<MetaType> metaTypeList = new List<MetaType>();

                        foreach (Widget w in this.textviews)
                        {
                            if (w.GetType() == typeof(MovableCasCalcView))
                            {
                                MetaType metaType = new MetaType();
                                MovableCasCalcView calcView = (MovableCasCalcView)w;
                                metaType.type = typeof(MovableCasCalcView);
                                metaType.metastring = calcView.calcview.input.Text;
                                metaTypeList.Add(metaType);
                            }
                            else if (w.GetType() == typeof(MovableCasTextView))
                            {
                                MetaType metaType = new MetaType();
                                MovableCasTextView textView = (MovableCasTextView)w;
                                metaType.type = typeof(MovableCasTextView);
                                metaType.metastring = textView.textview.SerializeCasTextView();
                                metaTypeList.Add(metaType);
                            }
                        }

                        if (metaTypeList.Count != 0 && string.IsNullOrEmpty(FileNameEntry.Text) == false)
                        {
                            string serializedString = ImEx.Export.Serialize(metaTypeList);
                            this.user.student.AddCompleted(serializedString, FileNameEntry.Text);
                        }
                        Destroy();
                    };

                    grid.Attach(button, 1, 1 + i, 1, 1);
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