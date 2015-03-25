using System;
using System.Collections.Generic;

namespace TaskGen
{
    class MainClass
    {
        static void MakeTask ()
        {
            int varMin = 1;
            int varMax = 10;
            int varNum = 4;
            int opsNum = 2;

            List<string> Operators = new List<string> ();
            List<int> Numbers = new List<int> ();
            Random r = new Random ();

            switch (opsNum) {
            case 1:
                Operators.Add ("+");
                break;
            case 2:
                Operators.Add ("-");
                break;
            case 4: 
                Operators.Add ("*");
                break;
            case 8: 
                Operators.Add ("/");
                break;
            }

            for (int i = 0; i < varNum; i++) {
                Numbers.Add (r.Next (varMin, varMax));
            }
    
            Console.Write (Numbers[0]);
            for (int i = 0; i < varNum-1; i++) {
                Console.Write (Operators[i]);
                Console.Write (Numbers[i+1]);
            }

        }

        public static void Main ()
        {
            MakeTask ();
        }
    }
}
