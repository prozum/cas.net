using System;
using System.Collections.Generic;
using ImEx;
using Gtk;

namespace DesktopUI
{
    public class StudentAddCompletedWindow : Window
    {
        User user;
        TextViewList textviews;

        public StudentAddCompletedWindow(ref User user, ref TextViewList textviews)
            : base("Upload Completed Assignment")
        {
            this.user = user;
            this.textviews = textviews;

            SetDefaultSize(300, 300);

            Label textLabel = new Label("Filename:");
            Entry fileName = new Entry();

            Button buttonCancel = new Button("Cancel");
            buttonCancel.Clicked += delegate
            {
                Destroy();
            };

            Button buttonUpload = new Button("Upload");
            buttonUpload.Clicked += delegate
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

                if (metaTypeList.Count != 0 && string.IsNullOrEmpty(fileName.Text) == false)
                {
                    string serializedString = ImEx.Export.Serialize(metaTypeList);
                    this.user.student.AddCompleted(serializedString, fileName.Text);
                }
                Destroy();
            };

            Grid grid = new Grid();
            grid.Attach(textLabel, 1, 1, 1, 1);
            grid.Attach(fileName, 2, 1, 1, 1);
            grid.Attach(buttonCancel, 1, 2, 1, 1);
            grid.Attach(buttonUpload, 2, 2, 1, 1);

            Add(grid);

            ShowAll();
        }

    }
}

