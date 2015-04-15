using System;
using System.Collections.Generic;

namespace Ast
{
    public class Evaluator
    {
        public Dictionary<string, Expression> variableDefinitions = new Dictionary<string, Expression>();
        public Dictionary<string, Expression> functionDefinitions = new Dictionary<string, Expression>();
        public Dictionary<string, List<string>> functionParams = new Dictionary<string, List<string>>();
        public bool degrees = true;

        public Evaluator ()
        {
        }

        public EvalData Evaluation(string inputString)
        {
            var exp = Parser.Parse(this, inputString);

            if (exp is Assign)
            {
                if ((exp as Assign).left is UserDefinedFunction)
                {
                    var paramNames = new List<string>();

                    if (functionDefinitions.ContainsKey(((exp as Assign).left as UserDefinedFunction).identifier))
                    {
                        functionDefinitions.Remove(((exp as Assign).left as UserDefinedFunction).identifier);
                        functionParams.Remove(((exp as Assign).left as UserDefinedFunction).identifier);
                    }

                    foreach (var item in ((exp as Assign).left as Function).args)
                    {   
                        if (item is Symbol)
                        {
                            paramNames.Add((item as Symbol).identifier);
                        }
                        else
                        {
                            //return new Error("Evaluator> One arg in the function is not a symbol");
                            return new EvalData(EvalType.Error, "Evaluator> One arg in the function is not a symbol");
                        }
                    }

                    functionParams.Add(((exp as Assign).left as UserDefinedFunction).identifier, paramNames);
                    functionDefinitions.Add(((exp as Assign).left as UserDefinedFunction).identifier, (exp as Assign).right);

                    //return new Info("Evaluator> Function defined");
                    return new EvalData(EvalType.Info, "Evaluator> Function defined");
                }
                else if ((exp as Assign).left is Symbol)
                {
                    if (variableDefinitions.ContainsKey(((exp as Assign).left as Symbol).identifier))
                    {
                        variableDefinitions.Remove(((exp as Assign).left as Symbol).identifier);
                    }

                    variableDefinitions.Add(((exp as Assign).left as Symbol).identifier, (exp as Assign).right);

                    return new EvalData(EvalType.Info, "Evaluator> Variable defined");
                }
                else
                {
                    return new EvalData(EvalType.Error, "Evaluator> Left expression is not a variable or function");
                }
            }
            else if (exp is Error)
            {
                return new EvalData(EvalType.Error, exp.ToString());
            }
            else if (exp is Info)
            {
                return new EvalData(EvalType.Info, exp.ToString());
            }
            else
            {
                return new EvalData(EvalType.Print, (SimplifyExp(exp).Evaluate().ToString()));
            }
        }

        public static Expression SimplifyExp(Expression exp)
        {
            var prevExp = "";
            
            do
            {
                prevExp = exp.ToString();

                exp = exp.Simplify();
            } while (exp.ToString() != prevExp);

            return exp;
        }

        public static Expression ExpandExp(Expression exp)
        {
            var prevExp = "";

            do
            {
                prevExp = exp.ToString();

                exp = exp.Expand();
            } while (exp.ToString() != prevExp);

            return exp;
        }
    }
}

