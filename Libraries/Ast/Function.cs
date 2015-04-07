using System;
using System.Collections.Generic;

namespace Ast
{
    public class Function  : Expression
    {
        static string[] specialFunctionNames = { "cos", "sin", "tan", "sqrt" };
        public string identifier;
        public List<Expression> args = new List<Expression>();
        public Dictionary<string, Expression> tempDefinitions;

        public Function(string identifier, List<Expression> args)
        {
            this.identifier = identifier;
            this.args = args;
        }

        public override string ToString ()
        {
            string str = identifier + '(';

            for (int i = 0; i < args.Count; i++) 
            {
                str += args[i].ToString ();

                if (i < args.Count - 1) 
                {
                    str += ',';
                }
            }

            return str + ')';
        }

        public override Expression Evaluate()
        {
            List<string> functionParemNames;
            Expression res;

            tempDefinitions = new Dictionary<string, Expression>(evaluator.variableDefinitions);

            foreach (var item in specialFunctionNames)
            {
                if (item == identifier)
                {
                    return HardcodedFunctions();
                }
            }

            if (evaluator.functionParams.TryGetValue(identifier, out functionParemNames))
            {
                if (functionParemNames.Count == args.Count)
                {
                    for (int i = 0; i < functionParemNames.Count; i++)
                    {
                        if (tempDefinitions.ContainsKey(functionParemNames[i]))
                        {
                            tempDefinitions.Remove(functionParemNames[i]);
                        }

                        tempDefinitions.Add(functionParemNames[i], args[i]);
                    }

                    evaluator.functionDefinitions.TryGetValue(identifier, out res);

                    res.SetFunctionCall(this);

                    return res.Evaluate();
                }
                else if (functionParemNames.Count == 0)
                {
                    return new Error("Can't call function with 0 parameters");
                }
                else
                {
                    return new Error("Function has the wrong number for parameters");
                }
            }
            else
            {
                return new Error("Function has no definition");
            }

            throw new NotImplementedException();
        }

        private Expression HardcodedFunctions()
        {
            if (args.Count == 1)
            {
                switch (identifier)
                {
                    case "cos":
                        if (args[0].Evaluate() is Integer)
                        {
                            return new Irrational((decimal)Math.Cos((args[0].Evaluate() as Integer).value));
                        }
                        if (args[0].Evaluate() is Rational)
                        {
                            return new Irrational((decimal)Math.Cos((double)(args[0].Evaluate() as Rational).value.value));
                        }
                        if (args[0].Evaluate() is Irrational)
                        {
                            return new Irrational((decimal)Math.Cos((double)(args[0].Evaluate() as Irrational).value));
                        }

                        return new Error("Could not take cos of: " + args[0].ToString());

                    case "sin":
                        if (args[0].Evaluate() is Integer)
                        {
                            return new Irrational((decimal)Math.Sin((args[0].Evaluate() as Integer).value));
                        }
                        if (args[0].Evaluate() is Rational)
                        {
                            return new Irrational((decimal)Math.Sin((double)(args[0].Evaluate() as Rational).value.value));
                        }
                        if (args[0].Evaluate() is Irrational)
                        {
                            return new Irrational((decimal)Math.Sin((double)(args[0].Evaluate() as Irrational).value));
                        }

                        return new Error("Could not take cos of: " + args[0].ToString());

                    case "tan":
                        if (args[0].Evaluate() is Integer)
                        {
                            return new Irrational((decimal)Math.Tan((args[0].Evaluate() as Integer).value));
                        }
                        if (args[0].Evaluate() is Rational)
                        {
                            return new Irrational((decimal)Math.Tan((double)(args[0].Evaluate() as Rational).value.value));
                        }
                        if (args[0].Evaluate() is Irrational)
                        {
                            return new Irrational((decimal)Math.Tan((double)(args[0].Evaluate() as Irrational).value));
                        }

                        return new Error("Could not take cos of: " + args[0].ToString());

                    case "sqrt":
                        if (args[0].Evaluate() is Integer)
                        {
                            return new Irrational((decimal)Math.Sqrt((args[0].Evaluate() as Integer).value));
                        }
                        if (args[0].Evaluate() is Rational)
                        {
                            return new Irrational((decimal)Math.Sqrt((double)(args[0].Evaluate() as Rational).value.value));
                        }
                        if (args[0].Evaluate() is Irrational)
                        {
                            return new Irrational((decimal)Math.Sqrt((double)(args[0].Evaluate() as Irrational).value));
                        }

                        return new Error("Could not take cos of: " + args[0].ToString());

                    default:
                        return new Error("Function has the wrong number for parameters");
                }
            }
            else
            {
                return new Error("Function has the wrong number for parameters");
            }
        }
    }
}

