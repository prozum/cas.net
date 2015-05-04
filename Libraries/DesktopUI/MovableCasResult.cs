using System;

namespace DesktopUI
{
    public class MovableCasResult : MovableCasTextView
    {
        public CasResult casresult;
        User user;

        public MovableCasResult(User user, string answer, string facit)
            : base("", false)
        {
            this.user = user;

            casresult = new CasResult(user, answer, facit);

            Remove(textview);
            textview = null;
            Attach(casresult, 1, 1, 1, 1);
        }
    }
}

