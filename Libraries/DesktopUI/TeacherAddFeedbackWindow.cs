using System;
using Gtk;
using System.Collections.Generic;
using ImEx;

namespace DesktopUI
{
    // Handles when the teacher gives feedback to the students
    public class TeacherAddFeedbackWindow : Window
    {
        User user;
        TextViewList textviews;
        string Filename;
        string className;

        // Constructor for teacheraddfeedbackwindow
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

                // Packs the workspace into a single string for easy transfer

				foreach (Widget w in this.textviews)
				{
                    if (w.GetType() == typeof(MovableCasCalcView))
                    {
                        MetaType metaType = new MetaType();
                        MovableCasCalcView calcView = (MovableCasCalcView)w;
                        metaType.type = typeof(MovableCasCalcView);
                        metaType.metastring = calcView.calcview.input.Text;
                        metaType.locked = calcView.textview.locked;
                        metaTypeList.Add(metaType);
                    }
                    else if (w is MovableCasCalcMulitlineView)
                    {
                        MetaType metaType = new MetaType();
                        MovableCasCalcMulitlineView calcview = (MovableCasCalcMulitlineView)w;
                        metaType.type = typeof(MovableCasCalcMulitlineView);
                        metaType.metastring = calcview.calcview.SerializeCasTextView();
                        metaType.locked = calcview.textview.locked;
                        metaTypeList.Add(metaType);
                    }
                    else if (w is MovableCasResult)
                    {
                        MetaType metaType = new MetaType();
                        MovableCasResult casres = (MovableCasResult)w;
                        metaType.type = typeof(MovableCasResult);
                        metaType.metastring = Export.Serialize(casres.casresult.facitContainer);
                        metaType.locked = casres.textview.locked;
                        metaTypeList.Add(metaType);
                    }
                    else if (w.GetType() == typeof(MovableCasTextView))
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

                                MessageDialog ms = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, "Added feedback");
                                ms.Run();
                                ms.Destroy();

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

