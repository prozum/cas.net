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

        static void PrintMenu (int varMin, int varMax, int varNum)
        {
            Console.WriteLine ("Main Menu:");
            Console.WriteLine ("1. Generate Task");
            Console.WriteLine ("2. Set varMin (is {0})", varMin);
            Console.WriteLine ("3. Set varMax (is {0})", varMax);
            Console.WriteLine ("4. Set varNum (is {0})", varNum);
        }

        public static void Main ()
        {
            string task;
            string input = "";
            int varMin = 1;
            int varMax = 10;
            int varNum = 2;
            int opsNum = varNum-1;     

            ConsoleKeyInfo In;

            PrintMenu (varMin, varMax, varNum);

            do {
                In = Console.ReadKey (true);

                Console.Clear ();
                PrintMenu (varMin, varMax, varNum);

                if (In.Key == ConsoleKey.D1) {
                    task = MakeTask (varMin, varMax, varNum, opsNum);
                    Console.Clear ();

                    Console.WriteLine (task);
                } else if (In.Key == ConsoleKey.D2) {
                    Console.Clear ();
                    Console.Write ("enter new value: ");
                    input = Console.ReadLine ();
                    int.TryParse (input, out varMin);
                } else if (In.Key == ConsoleKey.D3) {
                    Console.Clear ();
                    Console.Write ("enter new value: ");
                    input = Console.ReadLine ();
                    int.TryParse (input, out varMax);
                } else if (In.Key == ConsoleKey.D4) {
                    Console.Clear ();
                    Console.Write ("enter new value: ");
                    input = Console.ReadLine ();
                    int.TryParse (input, out varNum);
                }
       
            } while (In.Key != ConsoleKey.Escape);                
        }
    }
}