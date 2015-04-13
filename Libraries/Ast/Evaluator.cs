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

        public Expression Evaluation(string inputString)
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
                            paramNames.Add((item as Symbol).symbol);
                        } 
                        else
                        {
                            return new Error("One arg in the function is not a symbol");
                        }
                    }

                    functionParams.Add(((exp as Assign).left as UserDefinedFunction).identifier, paramNames);
                    functionDefinitions.Add(((exp as Assign).left as UserDefinedFunction).identifier, (exp as Assign).right);

                    return new Error("Function defined");
                }
                else if ((exp as Assign).left is Symbol)
                {
                    if (variableDefinitions.ContainsKey(((exp as Assign).left as Symbol).symbol))
                    {
                        variableDefinitions.Remove(((exp as Assign).left as Symbol).symbol);
                    }

                    variableDefinitions.Add(((exp as Assign).left as Symbol).symbol, (exp as Assign).right);

                    return new Error("Variable defined");
                }
                else
                {
                    return new Error("Left expression is not a variable or function");
                }
            }
            else if (exp is Simplify)
            {
                return new Error((exp as Simplify).Evaluate().ToString());
            }
            else if (exp is Expand)
            {
                return new Error((exp as Expand).Evaluate().ToString());
            }
            else
            {
                //return new Error(SimplifyExp(exp).ToString());
                return SimplifyExp(exp).Evaluate();
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

