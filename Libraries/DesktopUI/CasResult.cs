using System;
using Gtk;

namespace DesktopUI
{
    public class CasResult : Grid
    {
        public Entry entryFasitSet;
        public Entry entryFasitGet;

        public Label labelFacitSet;
        public Label labelFacitGet;

        CheckButton checkShowCorrect;
        Label labelCorrect;

        User user;

        public FacitContainer facitContainer = new FacitContainer();

        public bool correct = false;

        public class FacitContainer
        {

            public string facit;
            public string answer;

            public FacitContainer()
            {
                facit = "";
                answer = "";
            }

        }

        public CasResult(User user, string answer, string facit)
            : base()
        {
            this.user = user;
            this.facitContainer.answer = answer;
            this.facitContainer.facit = facit;

            entryFasitGet = new Entry();
            entryFasitSet = new Entry();

            entryFasitGet.Text = answer;
            entryFasitSet.Text = facit;

            labelFacitGet = new Label("Result:");
            labelFacitSet = new Label("Set facit:");

            checkShowCorrect = new CheckButton("Show students if correct");
            labelCorrect = new Label("");

            entryFasitGet.Changed += delegate
            {
                facitContainer.answer = entryFasitGet.Text;
                System.Threading.Thread thread = new System.Threading.Thread(ThreadCheckAnswer);
                thread.Start();
            };

            entryFasitSet.Changed += delegate
            {
                facitContainer.facit = entryFasitSet.Text;
            };

            checkShowCorrect.Toggled += delegate
            {
                correct = !correct;
            };

            if (user.privilege == 1)
            {
                Attach(labelFacitSet, 1, 1, 1, 1);
                Attach(entryFasitSet, 2, 1, 1, 1);
                Attach(labelFacitGet, 1, 2, 1, 1);
                Attach(entryFasitGet, 2, 2, 1, 1);

                Attach(checkShowCorrect, 1, 3, 2, 1);
            }
            if (user.privilege <= 0)
            {
                Attach(labelFacitGet, 1, 1, 1, 1);
                Attach(entryFasitGet, 2, 1, 1, 1);

                Attach(labelCorrect, 1, 2, 2, 1);
            }
        }

        void ThreadCheckAnswer()
        {
            if(entryFasitGet.Text.Equals(facitContainer.facit) == true)
            {
                System.Threading.Thread.Sleep(5000);
            }
            labelCorrect.Text = entryFasitGet.Text.Equals(facitContainer.facit) ? "Correct" : "Wrong";
            System.Threading.Thread.CurrentThread.Abort();
        }
    }
}

