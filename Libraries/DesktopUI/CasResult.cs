using System;
using Gtk;

namespace DesktopUI
{
    public class CasResult : Grid
    {
        Entry entryFasitSet;
        Entry entryFasitGet;

        Label labelFacitSet;
        Label labelFacigGet;

        CheckButton checkShowCorrect;
        Label labelCorrect;

        User user;

        bool correct = false;
        string answer;

        public CasResult(User user)
            : base()
        {
            this.user = user;

            entryFasitGet = new Entry();
            entryFasitSet = new Entry();

            labelFacigGet = new Label("Result:");
            labelFacitSet = new Label("Set result:");

            checkShowCorrect = new CheckButton();
            labelCorrect = new Label("");

            entryFasitGet.Changed += delegate
            {
                if (entryFasitGet.Text.Equals(answer))
                {
                    labelCorrect.Text = "Correct";
                }
                else
                {
                    labelCorrect.Text = "Wrong";    
                }
            };

            entryFasitSet.Changed += delegate
            {
                answer = entryFasitSet.Text;
            };

            checkShowCorrect.Toggled += delegate
            {
                correct = !correct;
            };

            if (user.privilege == 1)
            {
                Attach(labelFacitSet, 1, 1, 1, 1);
                Attach(entryFasitSet, 2, 1, 1, 1);
                Attach(checkShowCorrect, 1, 2, 2, 1);
            }
            if (user.privilege == 0)
            {
                Attach(labelFacitSet, 1, 1, 1, 1);
                Attach(entryFasitSet, 2, 1, 1, 1);

                if (correct == true)
                {
                    Attach(labelCorrect, 1, 2, 2, 1);
                }

            }
        }
    }
}

