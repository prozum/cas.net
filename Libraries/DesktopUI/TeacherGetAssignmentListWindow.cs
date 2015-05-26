using System;
using Gtk;

namespace DesktopUI
{
    // Gives the teacher a list of assignments
    public class TeacherGetAssignmentListWindow : Window
    {
        User user;
        TextViewList textviews;

        public TeacherGetAssignmentListWindow(User user, ref TextViewList textviews)
            : base("Assignment List")
        {
            this.user = user;
            this.textviews = textviews;

            string[] assignmentList = user.teacher.GetAssignmentList();

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            VBox vbox = new VBox(false, 2);

            // Creates a list of lines, where the teacher can get the complete work of the student, and add feedback for it.
            foreach (var item in assignmentList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Label label = new Label(item);

                    Button GetCompleted = new Button("Get Completed");
                    GetCompleted.Clicked += (sender, e) => new TeacherGetCompletedListWindow(this.user, ref this.textviews, item);
                    GetCompleted.Clicked += (sender, e) => this.Destroy();

                    Button AddFeedback = new Button("Add Feedback");
                    AddFeedback.Clicked += (sender, e) => new TeacherAddFeedbackWindow(this.user, this.textviews, item);
                    GetCompleted.Clicked += (sender, e) => this.Destroy();

                    HBox hbox = new HBox(false, 2);

                    hbox.Add(label);
                    hbox.Add(GetCompleted);
                    hbox.Add(AddFeedback);
                    vbox.Add(hbox);
                }
            }

            scrolledWindow.Add(vbox);
            Add(scrolledWindow);

            SetDefaultSize(300, 200);

            ShowAll();
        }
    }
}