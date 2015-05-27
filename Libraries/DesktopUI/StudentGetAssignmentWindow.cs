using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    // Window for students to get assignments
    public class StudentGetAssignmentWindow : Window
    {
        User user;
        TextViewList textviews;
        string Filename;

        public StudentGetAssignmentWindow(User user, ref TextViewList textviews, string Filename)
            : base("Get Assignment")
        {
            this.user = user;
            this.textviews = textviews;
            this.Filename = Filename;

            SetSizeRequest(300, 300);
            string assignment = this.user.student.GetAssignment(this.Filename);

            List<MetaType> metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(assignment);

            this.textviews.castextviews.Clear();

            // Loads each element into the workspace

            foreach (var metaItem in metaTypeList)
            {
                if (metaItem.type == typeof(MovableCasCalcView))
                {
                    this.textviews.InsertCalcView(metaItem.metastring, metaItem.locked);
                }
                else if(metaItem.type == typeof(MovableCasCalcMulitlineView))
                {
                    this.textviews.InsertCalcMultilineView(metaItem.metastring, metaItem.locked);
                }
                else if(metaItem.type == typeof(MovableCasResult))
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

            MessageDialog ms = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, "Got assignment");
            ms.Run();
            ms.Destroy();

            Destroy();
        }
    }
}

