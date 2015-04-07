using System;
using System.Collections.Generic;

namespace Ast
{
	public class Evaluator
	{
        public Dictionary<string, Expression> variableDefinitions = new Dictionary<string, Expression>();
        public Dictionary<string, Expression> functionDefinitions = new Dictionary<string, Expression>();
        public Dictionary<string, List<string>> functionParams = new Dictionary<string, List<string>>();

		public Evaluator ()
		{
		}

        public Expression Evaluation(string inputString)
        {
            var exp = Parser.Parse(this, inputString);

            if (exp is Assign)
            {
                if ((exp as Assign).left is Function)
                {
                    var paramNames = new List<string>();

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

                    functionParams.Add(((exp as Assign).left as Function).identifier, paramNames);
                    functionDefinitions.Add(((exp as Assign).left as Function).identifier, (exp as Assign).right);

                    return new Error("Function defined");
                }
                else if ((exp as Assign).left is Symbol)
                {
                    variableDefinitions.Add(((exp as Assign).left as Symbol).symbol, (exp as Assign).right);

                    return new Error("Varialbe defined");
                }
                else
                {
                    return new Error("Left expression is not a variable or function");
                }
            }
            else
            {
                return exp.Evaluate();
            }
        }

        public Expression Expand(string inputString)
        {
            var exp = Parser.Parse(this, inputString);
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

