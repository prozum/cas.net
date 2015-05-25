using System;
using System.Collections.Generic;

namespace Ast
{
    public class IfExpr : Expression
    {
        public List<Expression> Conditions = new List<Expression>();
        public List<Expression> Expressions = new List<Expression>();

        public IfExpr (Scope scope)
        { 
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            Expression res;

            for (int i = 0; i < Conditions.Count; i++)
            {
                res = Conditions[i].ReduceEvaluate();

                if (CurScope.GetBool("debug"))
                    CurScope.SideEffects.Add(new DebugData("Debug if cond["+i+"]: "+Conditions[i]+" = "+res));

                if (CurScope.Error)
                    return Constant.Null;

                if (res is Boolean)
                {
                    // If true
                    if (res as Boolean)
                    {
                        res = Expressions[i].ReduceEvaluate();
                        if (CurScope.GetBool("debug"))
                            CurScope.SideEffects.Add(new DebugData("Debug if expr[" + i + "]: " + Expressions[i] + " = " + res));
                        return Constant.Null;
                    }
                }
                else
                {
                    CurScope.Errors.Add(new ErrorData("Condition " + i + ": " + Conditions[i] + " does not return bool"));
                    return Constant.Null;
                }
            }

            if (Expressions.Count > Conditions.Count)
            {
                res = Expressions[Expressions.Count - 1].Evaluate();
                if (CurScope.GetBool("debug"))
                    CurScope.SideEffects.Add(new DebugData("Debug if["+(Expressions.Count-1)+"]: "+Expressions[Expressions.Count-1]+" = "+res));
            }

            return Constant.Null;
        }

        public override string ToString()
        {
            int i = 0;
            string str = "if " + Conditions[i].ToString() + ":" + Expressions[i].ToString();

            for (i = 1; i < Conditions.Count; i++)
            {
                str += "elif " + Conditions[i].ToString() + ":" + Expressions[i].ToString();
            }

            if (Conditions.Count + 1 == Expressions.Count)
                str += "else:" + Expressions[i].ToString();

            return str;
        }
    }
}

