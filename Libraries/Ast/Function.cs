using System;
using System.Collections.Generic;

namespace Ast
{
	public class Function  : Expression
	{
		public string identifier;
        public List<Expression> args = new List<Expression>();
        public Dictionary<string, List<string>> functionParams;
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
	}
}

