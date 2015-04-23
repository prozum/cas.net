using System;
using Gtk;
using ImEx;
using System.Collections.Generic;

namespace DesktopUI
{
    public class StudentAddCompletedMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;
        List<MetaType> metaTypeList = new List<MetaType>();

        public StudentAddCompletedMenuItem(ref User user, ref TextViewList textviews)
            : base("Add Completed")
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

            UploadCompletedWindow();
        }

        void UploadCompletedWindow()
        {
            Window window = new Window("Upload Completed Assignment");

            window.SetDefaultSize(300, 300);

            Label textLabel = new Label("Filename:");
            Entry fileName = new Entry();

            Button buttonCancel = new Button("Cancel");
            buttonCancel.Clicked += delegate
            {
                window.Destroy();
            };

            Button buttonUpload = new Button("Upload");
            buttonUpload.Clicked += delegate
            {
                if (metaTypeList.Count != 0 && string.IsNullOrEmpty(fileName.Text) == false)
                {
                    string serializedString = ImEx.Export.Serialize(metaTypeList);
                    user.student.AddCompleted(serializedString, fileName.Text);
                }
                window.Destroy();
            };

            Grid grid = new Grid();
            grid.Attach(textLabel, 1, 1, 1, 1);
            grid.Attach(fileName, 2, 1, 1, 1);
            grid.Attach(buttonCancel, 1, 2, 1, 1);
            grid.Attach(buttonUpload, 2, 2, 1, 1);

            window.Add(grid);

            window.ShowAll();
        }

    }
}

