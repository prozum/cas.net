// TODO 
//Omkreds, enhedskonvertering, areal, procentregning
//afrunding, vinkler
//(brøker?)

using System;
using System.Collections.Generic;

namespace TaskGen
{
    class MainClass
    {
        static string MakeCalcTask (int varMin, int varMax, int varNum)
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

            ConsoleKeyInfo In;

            PrintMenu (varMin, varMax, varNum);

            do {
                In = Console.ReadKey (true);

                Console.Clear ();
                PrintMenu (varMin, varMax, varNum);

                if (In.Key == ConsoleKey.D1) {
                    task = MakeCalcTask (varMin, varMax, varNum);
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
