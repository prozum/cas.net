using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using Ast;

namespace DesktopUI
{
    // Window for students to get feedback
    public sealed class StudentGetFeedbackWindow : Window
    {
        User user;
        TextViewList textviews;
        string Filename;

        public StudentGetFeedbackWindow(User user, ref TextViewList textviews, string Filename)
            : base("Feedback")
        {
            this.user = user;
            this.textviews = textviews;
            this.Filename = Filename;

            string assignment = this.user.student.GetFeedback(this.Filename);

            List<MetaType> metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(assignment);

            this.textviews.castextviews.Clear();

            // Loads each element in the list into the workspace

            foreach (var metaItem in metaTypeList)
            {
                if (metaItem.type == typeof(MovableCasCalcView))
                {
                    this.textviews.InsertCalcView(metaItem.metastring, metaItem.locked);
                }
                else if (metaItem.type == typeof(MovableCasCalcMulitlineView))
                {
                    this.textviews.InsertCalcMultilineView(metaItem.metastring, metaItem.locked);
                }
                else if (metaItem.type == typeof(MovableCasResult))
                {
                    CasResult.FacitContainer facitcontainer = Import.DeserializeString<CasResult.FacitContainer>(metaItem.metastring);
                    this.textviews.InsertResult(facitcontainer.answer, facitcontainer.facit);
                }
                else if (metaItem.type == typeof(MovableCasTextView))
                {
                    this.textviews.InsertTextView(metaItem.metastring, metaItem.locked, -1);
                }
            }

            this.textviews.castextviews.Reverse();

            this.textviews.Clear();
            this.textviews.Redraw();
            this.textviews.Reevaluate();
            this.textviews.ShowAll();
        }
    }
}

