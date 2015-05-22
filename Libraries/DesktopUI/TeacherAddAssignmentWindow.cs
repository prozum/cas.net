using System;
using Gtk;
using ImEx;
using System.Collections.Generic;

namespace DesktopUI
{
    // Handles when the teacher adds a new assignment
    public class TeacherAddAssignmentWindow : Window
    {
        User user;
        TextViewList textviews;

        // Constructor for teacheraddassignmentwindow
        public TeacherAddAssignmentWindow(User user, TextViewList textviews)
            : base("Add Assignment")
        {
            this.user = user;
            this.textviews = textviews;

            SetSizeRequest(300, 300);

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
                List<MetaType> metaTypeList = new List<MetaType>();

                CasTextViewSerializer serializer = new CasTextViewSerializer();

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
                    else if(w is MovableCasCalcMulitlineView)
                    {
                        MetaType metaType = new MetaType();
                        MovableCasCalcMulitlineView calcview = (MovableCasCalcMulitlineView)w;
                        metaType.type = typeof(MovableCasCalcMulitlineView);
                        metaType.metastring = serializer.SerializeCasTextView(calcview.calcview.input);
                        metaType.locked = calcview.textview.locked;
                        metaTypeList.Add(metaType);
                    }
                    else if(w is MovableCasResult)
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
                        metaType.metastring = serializer.SerializeCasTextView(textView.textview);
                        metaType.locked = textView.textview.locked;
                        metaTypeList.Add(metaType);
                    }
                }
                
                if (metaTypeList.Count != 0
                    && !string.IsNullOrEmpty(name.Text)
                    && !string.IsNullOrEmpty(grad.Text))
                {
                    string Assignment = Export.Serialize(metaTypeList);
                    this.user.teacher.AddAssignment(Assignment, name.Text, grad.Text);

                    Destroy();
                }
                else
                {
                    warningLabel.Text = "Warning, upload invalid";
                }
            };

            Button cancelButton = new Button("Cancel");
            cancelButton.Clicked += delegate
            {
                Destroy();
            };

            grid.Attach(nameLabel, 1, 1, 1, 1);
            grid.Attach(name, 2, 1, 1, 1);
            grid.Attach(classLabel, 1, 2, 1, 1);
            grid.Attach(grad, 2, 2, 1, 1);
            grid.Attach(warningLabel, 1, 3, 2, 1);
            grid.Attach(uploadButton, 2, 4, 1, 1);
            grid.Attach(cancelButton, 1, 4, 1, 1);

            Add(grid);
            ShowAll();
        }
    }
}