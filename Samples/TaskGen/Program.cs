// TODO 
//Omkreds, enhedskonvertering, procentregning
//afrunding, vinkler
//(brøker?)

using System;
using System.Collections.Generic;
using Ast;

namespace TaskGen
{
    class MainClass
    {
        static string makeUnitTask (int varMin, int varMax)
        {
            //1-Distance
            int metre = 1;
            double centimetre = metre * 0.01;
            double millimetre = metre * 0.001;
            //2-weight
            double kilogram = 1;
            double gram = kilogram * 0.01;
            //3-volume
            double cubicMetre = 1;
            double litre = cubicMetre * 0.001;

            Random r = new Random (Guid.NewGuid ().GetHashCode ());
            string task = "";

            string unit1 = "";
            string unit2 = "";
            int val;
            int type = r.Next (1, 4);

            switch (type) {
            case 1:
                type = r.Next (1, 4);
                switch (type) {
                case 1:
                    unit1 = "metre";
                    break;
                case 2:
                    unit1 = "centimetre";
                    break;
                case 3:
                    unit1 = "millimetre";
                    break;
                }
                type = r.Next (1, 3);

                switch (unit1) {
                case "metre":
                    switch (type) {
                    case 1:
                        unit2 = "centimetre";
                        break;
                    case 2:
                        unit2 = "millimetre";
                        break;
                    }
                    break;
                case "centimetre":
                    switch (type) {
                    case 1:
                        unit2 = "metre";
                        break;
                    case 2:
                        unit2 = "millimetre";
                        break;
                    }
                    break;
                case "millimetre":
                    switch (type) {
                    case 1:
                        unit2 = "metre";
                        break;
                    case 2:
                        unit2 = "centimetre";
                        break;
                    }
                    break;
                }
                break;

            case 2:
                type = r.Next (1, 3);

                switch (type) {
                case 1:
                    unit1 = "kilogram";
                    break;
                case 2:
                    unit1 = "gram";
                    break;
                }
                switch (unit1) {
                case "kilogram":
                    unit2 = "gram";
                    break;
                case "gram":
                    unit2 = "kilogram";
                    break;
                }
                break;
            case 3:
                type = r.Next (1, 3);

                switch (type) {
                case 1:
                    unit1 = "cubicmetre";
                    break;
                case 2:
                    unit1 = "litre";
                    break;
                }
                switch (unit1) {
                case "cubicmetre":
                    unit2 = "litre";
                    break;
                case "litre":
                    unit2 = "cubicmetre";
                    break;
                }
                break;
            }

            val = r.Next (varMin, varMax);

            Console.WriteLine ("how many {0}(s) is {1} {2}(s)", unit1, val, unit2);

            return task;

        }

        static string makeAreaTask (int varMin, int varMax)
        {
            Random r = new Random (Guid.NewGuid ().GetHashCode ());
            string task = "";
            int iSide1, iSide2;

            iSide1 = r.Next (varMin, varMax);

            r = new Random (Guid.NewGuid ().GetHashCode ());

            iSide2 = r.Next (varMin, varMax);

            string side1 = iSide1.ToString ();
            string side2 = iSide2.ToString ();

            task = side1 + "*" + side2;

            return task;
        }

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

        static string GetAnswer (string task)
        {       
            string answer = Parser.Parse (task).Evaluate ().ToString ();

            return answer;
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

            makeUnitTask (4, 7);

            do {
                In = Console.ReadKey (true);

                Console.Clear ();
                PrintMenu (varMin, varMax, varNum);

                if (In.Key == ConsoleKey.D1) {
                    task = MakeCalcTask (varMin, varMax, varNum);
                    Console.Clear ();
                    Console.WriteLine (task);
                    Console.WriteLine (GetAnswer (task));
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
                } else if (In.Key == ConsoleKey.A) {
                    task = makeAreaTask (varMin, varMax);
                    Console.Clear ();
                    Console.WriteLine (task);
                }
       
            } while (In.Key != ConsoleKey.Escape);                
        }
    }
}
