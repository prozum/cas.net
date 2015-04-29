using System;
using Gtk;
using System.Collections.Generic;
using ImEx;

namespace DesktopUI
{
    public class TeacherAddFeedbackWindow : Window
    {
        User user;
        TextViewList textviews;

        public TeacherAddFeedbackWindow(ref User user, ref TextViewList textviews)
            : base("Add Feedback")
        {
            this.user = user;
            this.textviews = textviews;

            SetSizeRequest(300, 300);

			Grid grid = new Grid();

            Label labClass = new Label("Class:");
            Label labFilename = new Label("Filename:");

            Entry entClass = new Entry();
            Entry entFilename = new Entry();

            Button buttonCancel = new Button("Cancel");
            buttonCancel.Clicked += delegate
            {
                Destroy();
            };

            Button buttonFeedback = new Button("Feedback");
            buttonFeedback.Clicked += delegate
            {
				string feedbackString = String.Empty;
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

				if (metaTypeList.Count != 0
					&& string.IsNullOrEmpty(entClass.Text) == false
					&& string.IsNullOrEmpty(entFilename.Text) == false)
				{
					feedbackString = Export.Serialize(metaTypeList);           
				}

				string[] StudentList = this.user.teacher.GetCompletedList(entFilename.Text, entClass.Text);

				grid.Destroy();
				grid = new Grid();

				for (int i = 0; i < StudentList.Length/2; i++)
				{
						int j = 2*i;
						Button button = new Button(StudentList[j]);
						button.Clicked += delegate
							{
								this.user.teacher.AddFeedback(feedbackString, entFilename.Text, StudentList[j], entClass.Text);
								Destroy();
							};
						grid.Attach(button, 1, 1+i, 1, 1);
				}          
				
				Add(grid);
                ShowAll();
            };

            grid.Attach(labClass, 1, 1, 1, 1);
            grid.Attach(entClass, 2, 1, 1, 1);
            grid.Attach(labFilename, 1, 2, 1, 1);
            grid.Attach(entFilename, 2, 2, 1, 1);
            grid.Attach(buttonCancel, 1, 3, 1, 1);
            grid.Attach(buttonFeedback, 2, 3, 1, 1);

            Add(grid);

            ShowAll();
        }
    }
}

