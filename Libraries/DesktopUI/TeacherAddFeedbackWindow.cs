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
        string Filename;
        string className;

        public TeacherAddFeedbackWindow(User user, TextViewList textviews, string Filename)
            : base("Add Feedback")
        {
            this.user = user;
            this.textviews = textviews;
            this.Filename = Filename;

            SetSizeRequest(300, 300);

			Grid grid = new Grid();

            Label labClass = new Label("Class:");
            Entry entClass = new Entry();

            entClass.Changed += (e, arg) => { className = entClass.Text; };

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
					else if (w is MovableLockedCasTextView)
					{
						MetaType metaType = new MetaType();
						MovableCasTextView textView = (MovableCasTextView)w;
						metaType.type = typeof(MovableCasTextView);
						metaType.metastring = textView.textview.SerializeCasTextView();
                        metaType.locked = textView.textview.locked;
						metaTypeList.Add(metaType);
					}
				}

				if (metaTypeList.Count != 0
                    && string.IsNullOrEmpty(className) == false
                    && string.IsNullOrEmpty(this.Filename) == false)
				{
					feedbackString = Export.Serialize(metaTypeList);           
				}

                string[] StudentList = this.user.teacher.GetCompletedList(this.Filename, className);

				grid.Destroy();
				grid = new Grid();

				for (int i = 0; i < StudentList.Length/2; i++)
				{
						int j = 2*i;
						Button button = new Button(StudentList[j]);
						button.Clicked += delegate
							{
                                this.user.teacher.AddFeedback(feedbackString, this.Filename, StudentList[j], className);
								Destroy();
							};
						grid.Attach(button, 1, 1+i, 1, 1);
				}          
				
				Add(grid);
                ShowAll();
            };

            grid.Attach(labClass, 1, 1, 1, 1);
            grid.Attach(entClass, 2, 1, 1, 1);
            grid.Attach(buttonCancel, 1, 3, 1, 1);
            grid.Attach(buttonFeedback, 2, 3, 1, 1);

            Add(grid);

            ShowAll();
        }
    }
}

