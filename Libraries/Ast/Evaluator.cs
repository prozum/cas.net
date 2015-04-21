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

        public Parser parser;

        public Evaluator ()
        {
            parser = new Parser (this);
        }

        public EvalData Evaluation(string inputString)
        {
            var exp = parser.Parse(this, inputString);

            if (exp is Assign)
            {
                if ((exp as Assign).Left is UserDefinedFunction)
                {
                    if ((exp as Assign).Right.ContainsVariable((exp as Assign).Left as UserDefinedFunction))
                    {
                        return new MsgData(MsgType.Error, "Evaluator> Can't define function as it self");
                    }

                    var paramNames = new List<string>();

                    if (functionDefinitions.ContainsKey(((exp as Assign).Left as UserDefinedFunction).identifier))
                    {
                        functionDefinitions.Remove(((exp as Assign).Left as UserDefinedFunction).identifier);
                        functionParams.Remove(((exp as Assign).Left as UserDefinedFunction).identifier);
                    }

                    foreach (var item in ((exp as Assign).Left as Function).args)
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

                    functionParams.Add(((exp as Assign).Left as UserDefinedFunction).identifier, paramNames);
                    functionDefinitions.Add(((exp as Assign).Left as UserDefinedFunction).identifier, (exp as Assign).Right);

                    return new MsgData(MsgType.Info, "Evaluator> Function defined");
                }
                else if ((exp as Assign).Left is Symbol)
                {
                    if ((exp as Assign).Right.ContainsVariable((exp as Assign).Left as Symbol))
                    {
                        return new MsgData(MsgType.Error, "Evaluator> Can't define symbol as it self");
                    }

                    if (variableDefinitions.ContainsKey(((exp as Assign).Left as Symbol).identifier))
                    {
                        variableDefinitions.Remove(((exp as Assign).Left as Symbol).identifier);
                    }

                    variableDefinitions.Add(((exp as Assign).Left as Symbol).identifier, (exp as Assign).Right);

                    return new MsgData(MsgType.Info, "Evaluator> Variable defined");
                }
                else
                {
                    return new MsgData(MsgType.Error, "Evaluator> Left expression is not a variable or function");
                }
            }
            else if (exp is Plot)
            {
                if ((exp as Plot).isArgsValid())
                    return new PlotData((Plot) exp);
                else
                    exp = new ArgError((Plot)exp);
            }
            else if (exp is Simplify || exp is Expand)
            {
                exp = exp.Evaluate();
            }
            else
            {
                exp = SimplifyExp(exp).Evaluate();
            }

            if (exp is Error)
            {
                return new MsgData(MsgType.Error, exp.ToString());
            }
            else if (exp is Info)
            {
                return new MsgData(MsgType.Info, exp.ToString());
            }
            else
            {
                return new MsgData(MsgType.Print, exp.ToString());
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

