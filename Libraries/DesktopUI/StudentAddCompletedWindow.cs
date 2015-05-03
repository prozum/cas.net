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
        string Filename;

        public StudentAddCompletedWindow(User user, TextViewList textviews, string Filename)
            : base("Upload Completed Assignment")
        {
            this.user = user;
            this.textviews = textviews;
            this.Filename = Filename;

            SetDefaultSize(300, 300);

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

            if (metaTypeList.Count != 0 && !string.IsNullOrEmpty(this.Filename))
            {
                string serializedString = ImEx.Export.Serialize(metaTypeList);
                this.user.student.AddCompleted(serializedString, this.Filename);
            }

            Destroy();
        }

    }
}

