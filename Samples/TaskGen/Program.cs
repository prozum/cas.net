using System;
using System.Collections.Generic;

namespace TaskGen
{
    class MainClass
    {


        static string MakeTask (int varMin, int varMax, int varNum, int opsNum)
        {
            List<string> Operators = new List<string> ();
            List<int> Numbers = new List<int> ();

            string task = "";

            Random r = new Random ();

            Operators.Clear ();
            Numbers.Clear ();

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
    
            task += Numbers[0];
            for (int i = 0; i < varNum-1; i++) {
                task += Operators [i];
                task += Numbers [i + 1];                  

            }
            return task;

        }            

        public static void Main ()
        {
            string task;
            int varMin = 1;
            int varMax = 10;
            int varNum = 2;
            int opsNum = 2;

            ConsoleKeyInfo In;

            Console.WriteLine ("Main Menu:");
            Console.WriteLine ("1. Generate Task");
            Console.WriteLine ("2. Task Settings");
            Console.ReadKey ();

            task = MakeTask (varMin, varMax, varNum, opsNum);
        }
    }
}