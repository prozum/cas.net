using System;
using System.Collections.Generic;
using ImEx;
using Gtk;

namespace DesktopUI
{
    // Window for student to add completed assignments
    public class StudentAddCompletedWindow : Window
    {
        User user;
        TextViewList textviews;
        string Filename;

        // Constructor for studentaddcompletedwindow
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
                if (w is MovableCasCalcView)
                {
                    MetaType metaType = new MetaType();
                    MovableCasCalcView calcView = (MovableCasCalcView)w;
                    metaType.type = typeof(MovableCasCalcView);
                    metaType.metastring = calcView.calcview.input.Text;
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
                else if(w is MovableCasResult)
                {
                    MetaType metaType = new MetaType();
                    MovableCasResult casres = (MovableCasResult)w;
                    metaType.type = typeof(MovableCasResult);
                    metaType.metastring = Export.Serialize(casres.casresult.facitContainer);
                    metaType.locked = casres.textview.locked;
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

            if (metaTypeList.Count != 0 && !string.IsNullOrEmpty(this.Filename))
            {
                string serializedString = ImEx.Export.Serialize(metaTypeList);
                this.user.student.AddCompleted(serializedString, this.Filename);
            }

            Destroy();
        }

    }
}

