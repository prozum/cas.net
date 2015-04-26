using System;
using Gtk;

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

			Grid grid = new Grid();

			Label FileNameLabel = new Label("Filename:");
			Entry FileNameEntry = new Entry();

			Label GradeLabel = new Label("Grade:");
			Entry GradeEntry = new Entry();

			Button CancelButton = new Button("Cancel");
			CancelButton.Clicked += delegate
				{
					Destroy();
				};

			Button DownloadButton = new Button("List of Completed Students");
			CancelButton.Clicked += delegate
				{
					StudentList = this.user.teacher.GetCompletedList(FileNameEntry.Text, GradeEntry.Text);

					foreach (Widget widget in grid)
					{
						grid.Remove(widget);
					}

					Destroy();
				};

			grid.Attach(FileNameLabel, 1, 1, 1 ,1);
			grid.Attach(FileNameEntry, 2, 1, 1, 1);

			grid.Attach(GradeLabel, 1, 2, 1, 1);
			grid.Attach(GradeEntry, 2, 2, 1, 1);

			grid.Attach(DownloadButton, 3, 1, 1, 1);
			grid.Attach(CancelButton, 3, 2, 1, 1);

			Add(grid);

			ShowAll();
		}
	}
}