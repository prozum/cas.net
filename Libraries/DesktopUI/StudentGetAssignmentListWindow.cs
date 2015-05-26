using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using Ast;

namespace DesktopUI
{
    // Window for students to get a list of all assignments
    public class StudentGetAssignmentListWindow : Window
    {
        User user;
        TextViewList textviews;

        public StudentGetAssignmentListWindow(User user, ref TextViewList textviews)
            : base("Get Assignment List")
        {
            this.user = user;
            this.textviews = textviews;

            string[] assignmentList = user.student.GetAssignmentList();

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            VBox vbox = new VBox(false, 2);
                                   
            // For each task on the server that the student has access to, creates a line where the student can get the assignment, upload the completed assignment, and get feedback should any have been given
            foreach (var assignment in assignmentList)
            {
                if (!string.IsNullOrEmpty(assignment))
                {
                    HBox hbox = new HBox(false, 2);

                    Label label = new Label(assignment);

                    Button GetAssignment = new Button("Get Assignment");
                    GetAssignment.Clicked += (sender, e) => new StudentGetAssignmentWindow(this.user, ref this.textviews, assignment);
                    GetAssignment.Clicked += (sender, e) => this.Destroy();

                    Button AddCompleted = new Button("Add Completed");
                    AddCompleted.Clicked += (sender, e) => new StudentAddCompletedWindow(this.user, this.textviews, assignment);
                    AddCompleted.Clicked += (sender, e) => this.Destroy();

                    Button GetFeedback = new Button("Get Feedback");
                    GetFeedback.Clicked += (sender, e) => new StudentGetFeedbackWindow(this.user, ref this.textviews, assignment);
                    GetFeedback.Clicked += (sender, e) => this.Destroy();

                    hbox.Add(label);
                    hbox.Add(GetAssignment);
                    hbox.Add(AddCompleted);
                    hbox.Add(GetFeedback);
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

