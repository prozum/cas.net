using System;
using Gtk;
using System.Collections.Generic;
using ImEx;

namespace DesktopUI
{
    public class TeacherAddAssignmentMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;
        List<MetaType> metaTypeList = new List<MetaType>();
        string filename;
        string grade;
        string uploadString;

        public TeacherAddAssignmentMenuItem(ref User user, ref TextViewList textviews)
            : base("Add Assignment")
        {
            this.user = user;
            this.textviews = textviews;

            this.Activated += delegate
            {
                OnClicked();
            };
        }

        void OnClicked()
        {
            foreach (Widget w in textviews)
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

            AddAssignmentWindow();
        }

        void AddAssignmentWindow()
        {
            Window window = new Window("Add Assignment");
            window.SetSizeRequest(300, 300);

            Grid grid = new Grid();

            Entry grad = new Entry();
            grad.WidthRequest = 200;
            Label classLabel = new Label("Class:");

            Entry name = new Entry();
            name.WidthRequest = 200;
            Label nameLabel = new Label("File name:");

            Label warningLabel = new Label();

            Button uploadButton = new Button("Upload");
            uploadButton.Clicked += delegate
            {
                if (metaTypeList.Count != 0
                    && string.IsNullOrEmpty(name.Text) == false
                    && string.IsNullOrEmpty(grad.Text) == false)
                {
                    filename = name.Text;
                    grade = grad.Text;

                    OnUploadClicked();
                    window.Destroy();
                }
                else
                {
                    warningLabel.Text = "Warning, upload invalid";
                }
            };

            Button cancelButton = new Button("Cancel");
            cancelButton.Clicked += delegate
            {
                window.Destroy();
            };

            grid.Attach(nameLabel, 1, 1, 1, 1);
            grid.Attach(name, 2, 1, 1, 1);
            grid.Attach(classLabel, 1, 2, 1, 1);
            grid.Attach(grad, 2, 2, 1, 1);
            grid.Attach(warningLabel, 1, 3, 2, 1);
            grid.Attach(uploadButton, 2, 4, 1, 1);
            grid.Attach(cancelButton, 1, 4, 1, 1);

            window.Add(grid);
            window.ShowAll();
        }

        void OnUploadClicked()
        {
            uploadString = Export.Serialize(metaTypeList);
            user.teacher.AddAssignment(uploadString, filename, grade);
        }
    }
}

