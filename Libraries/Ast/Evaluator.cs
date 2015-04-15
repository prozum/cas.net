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
                            return new MsgData(MsgType.Error, "Evaluator> One arg in the function is not a symbol");
                        }
                    }

                    functionParams.Add(((exp as Assign).left as UserDefinedFunction).identifier, paramNames);
                    functionDefinitions.Add(((exp as Assign).left as UserDefinedFunction).identifier, (exp as Assign).right);

                    return new MsgData(MsgType.Info, "Evaluator> Function defined");
                }
                else if ((exp as Assign).left is Symbol)
                {
                    if (variableDefinitions.ContainsKey(((exp as Assign).left as Symbol).identifier))
                    {
                        variableDefinitions.Remove(((exp as Assign).left as Symbol).identifier);
                    }

                    variableDefinitions.Add(((exp as Assign).left as Symbol).identifier, (exp as Assign).right);

                    return new MsgData(MsgType.Info, "Evaluator> Variable defined");
                }
                else
                {
                    return new MsgData(MsgType.Error, "Evaluator> Left expression is not a variable or function");
                }
            }
            else if (exp is Function)
            {
                return new MsgData(MsgType.Info, (exp as Function).Evaluate().ToString());
            }
            else if (exp is Error)
            {
                return new MsgData(MsgType.Error, exp.ToString());
            }
            else if (exp is Info)
            {
                return new MsgData(MsgType.Info, exp.ToString());
            }
            else
            {
                return new MsgData(MsgType.Print, (SimplifyExp(exp).ToString()));
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

