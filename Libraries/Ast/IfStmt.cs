using System;
using System.Collections.Generic;

namespace Ast
{
    public class IfStmt : Statement
    {
        public List<Expression> conditions = new List<Expression>();
        public List<Expression> expressions = new List<Expression>();

        public IfStmt (Scope scope) : base(scope)
        {
        }

        public override void Evaluate()
        {
            Expression res;

            for (int i = 0; i < conditions.Count; i++)
            {
                res = conditions[i].Evaluate();

                if (Scope.GetBool("debug"))
                    Scope.SideEffects.Add(new DebugData("Debug if["+i+"]: "+conditions[i]+" = "+res));
                    
                if (res is Error)
                {
                    Scope.Errors.Add(new ErrorData(res as Error));
                    return;
                }

                if (res is Boolean)
                {
                    res = expressions[i].Evaluate();
                    if (Scope.GetBool("debug"))
                        Scope.SideEffects.Add(new DebugData("Debug if[" + i + "]: " + conditions[i] + " = " + res));
                }
                else
                {
                    Scope.Errors.Add(new ErrorData("Condition " + i + ": " + conditions[i] + " does not return bool"));
                    return;
                }
            }

            if (expressions.Count > conditions.Count)
            {
                res = expressions[expressions.Count - 1].Evaluate();
                if (Scope.GetBool("debug"))
                    Scope.SideEffects.Add(new DebugData("Debug if["+(expressions.Count-1)+"]: "+conditions[expressions.Count-1]+" = "+res));
            }
        }

        public override string ToString()
        {
            int i = 0;
            string str = "if " + conditions[i].ToString() + ":" + expressions[i].ToString();

            for (i = 1; i < conditions.Count; i++)
            {
                str += "elif " + conditions[i].ToString() + ":" + expressions[i].ToString();
            }

            if (conditions.Count + 1 == expressions.Count)
                str += "else:" + expressions[i].ToString();

            return str;
        }
    }
}

