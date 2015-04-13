using System;
using System.Collections.Generic;

namespace TaskGenLib
{
    public static class TaskGen
    {
        public static string MakeCalcTask (int varMin, int varMax, int varNum)
        {
            List<string> Operators = new List<string> ();
            List<int> Numbers = new List<int> ();

            string task = "";

            Random r = new Random (Guid.NewGuid().GetHashCode());

            Operators.Clear ();
            Numbers.Clear ();

            int opsNum;

            Numbers.Add (r.Next (varMin, varMax));

            for (int i = 0; i < varNum - 1; i++)
            {
                Numbers.Add(r.Next(varMin, varMax));
                r = new Random (Guid.NewGuid().GetHashCode());

                opsNum = r.Next(1, 5);

                switch (opsNum)
                {
                case 1:
                    Operators.Add("+");
                    break;
                case 2:
                    Operators.Add("-");
                    break;
                case 3: 
                    Operators.Add("*");
                    break;
                case 4:
                    while (Numbers[i] == 0)
                    {
                        Numbers[i] = r.Next(varMin, varMax);
                    }
                    Operators.Add("/");
                    break;
                }
            }

            task += Numbers[0];
            for (int i = 0; i < varNum-1; i++) {
                task += Operators [i];
                task += Numbers [i + 1];                  

            }
            return task;
        }
    }
}
