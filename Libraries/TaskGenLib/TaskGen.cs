using System;
using System.Collections.Generic;
using Ast;

namespace TaskGenLib
{
    public static class TaskGen
    {
        enum Units {Distance, Weight, Volume};
        enum DistanceUnits {Metre, Centimetre, Millimetre};
        enum WeightUnits {Kilogram, gram};
        enum VolumeUnits {Cubicmetre, Litre};

        public static Task MakeUnitTask (int varMin, int varMax)
        {
            Random r = new Random (Guid.NewGuid ().GetHashCode ());
            
            string unit1 = "";
            string unit2 = "";
            double val;
            string answer = "";
            Units unit = (Units)r.Next (0, 2);
            DistanceUnits distance = (DistanceUnits)r.Next (0, 2);
            WeightUnits weight = (WeightUnits)r.Next (0, 1);
            VolumeUnits volume = (VolumeUnits)r.Next (0, 1);

            switch (unit) {
            case Units.Distance:
                switch (distance) {
                case DistanceUnits.Metre:
                    unit1 = "metres";
                    break;
                case DistanceUnits.Centimetre:
                    unit1 = "centimetres";
                    break;
                case DistanceUnits.Millimetre:
                    unit1 = "millimetres";
                    break;
                }
                distance = (DistanceUnits)r.Next(0, 3);

                switch (unit1) {
                case "metres":
                    while (unit2 == unit1 || unit2 == "") {
                        switch (distance) {
                        case DistanceUnits.Centimetre:
                            unit2 = "centimetres";
                            break;
                        case DistanceUnits.Millimetre:
                            unit2 = "millimetres";
                            break;
                        case DistanceUnits.Metre:
                            unit2 = "metres";
                            break;
                        }
                    }
                    break;
                case "centimetres":
                    while (unit2 == unit1 || unit2 == "") {
                        switch (distance) {
                        case DistanceUnits.Metre:
                            unit2 = "metres";
                            break;
                        case DistanceUnits.Millimetre:
                            unit2 = "millimetres";
                            break;
                        case DistanceUnits.Centimetre:
                            unit2 = "millimetres";
                            break;
                        }
                    }
                    break;
                case "millimetres":
                    switch (distance) {
                    case DistanceUnits.Metre:
                        unit2 = "metres";
                        break;
                    case DistanceUnits.Centimetre:
                        unit2 = "centimetres";
                        break;
                    case DistanceUnits.Millimetre:
                        unit2 = "millimetres";
                        break;
                    }
                    break;
                }
                break;

            case Units.Weight:
                switch (weight) {
                case WeightUnits.Kilogram:
                    unit1 = "kilograms";
                    break;
                case WeightUnits.gram:
                    unit1 = "grams";
                    break;
                }
                switch (unit1) {
                case "kilograms":
                    unit2 = "gram";
                    break;
                case "grams":
                    unit2 = "kilogram";
                    break;
                }
                break;
            case Units.Volume:
                switch (volume) {
                case VolumeUnits.Cubicmetre:
                    unit1 = "cubicmetres";
                    break;
                case VolumeUnits.Litre:
                    unit1 = "litres";
                    break;
                }
                switch (unit1) {
                case "cubicmetres":
                    unit2 = "litres";
                    break;
                case "litres":
                    unit2 = "cubicmetres";
                    break;
                }
                break;
            }

            val = r.Next (varMin, varMax);
            string taskS = "";
            taskS += "How many " + unit1 + " is " + val + " " + unit2 +"?";

            //conversions
            if (unit1 == "metres" && unit2 == "centimetres") {
                val = val * 0.01;
            } else if (unit1 == "metres" && unit2 == "millimetres") {
                val = val * 0.001;
            } else if (unit1 == "centimetres" && unit2 == "metres") {
                val = val * 100;
            } else if (unit1 == "centimetres" && unit2 == "millimetres") {
                val = val * 0.1;
            } else if (unit1 == "millimetres" && unit2 == "metres") {
                val = val * 1000;
            } else if (unit1 == "millimetres" && unit2 == "centimetres") {
                val = val * 10;
            } else if (unit1 == "litres") {
                val = val * 1000;
            } else if (unit1 == "cubicmetres") {
                val = val * 0.001;
            } else if (unit1 == "gram") {
                val = val * 1000;
            } else if (unit1 == "kilogram") {
                val = val * 0.001;
            }

            answer = val.ToString ();

            var task = new Task (taskS, GetAnswer (answer));

            return task;
        }
            
        public static Task MakeCalcTask (int varMin, int varMax, int varNum)
        {
            List<string> Operators = new List<string> ();
            List<int> Numbers = new List<int> ();

            string taskS = "";  

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

            taskS += Numbers[0];
            for (int i = 0; i < varNum-1; i++) {
                taskS += Operators [i];
                taskS += Numbers [i + 1];                  

            }

            var task = new Task (taskS, GetAnswer (taskS));

            return task;
        }

        static Expression GetAnswer (string task)
        {
            Evaluator eval = new Evaluator();
//            Expression answer = eval.Evaluation(task);

            return null;
        }

    }
}
