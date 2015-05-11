using System;
using System.Collections.Generic;

namespace Ast
{
    public class IfStmt : Expression
    {
        public List<Expression> conditions = new List<Expression>();
        public List<Expression> expressions = new List<Expression>();

        int curCond = 0;
        bool? resCond = null;

        public IfStmt()
        {
        }

        public override Expression Evaluate()
        {
            for(int i = 0; i < conditions.Count; i++)
            {
                var res = conditions[i].Evaluate();

                if (res is Boolean)
                {
                    if ((res as Boolean).@bool)
                        return expressions[i].Evaluate();

                    continue;
                }
                else
                    return res;
            }

            if (conditions.Count + 1 == expressions.Count)
                return expressions[expressions.Count - 1].Evaluate();

            throw new Exception("Something went wrong in the parser");
        }

        public override EvalData Step()
        {
            Expression res;

            if (resCond != true && curCond < conditions.Count)
            {
                res = conditions[curCond].Evaluate();

                if (res is Boolean)
                {
                    resCond = (res as Boolean).@bool;
                }

                if (resCond != true)
                    curCond++;

                return new DebugData("If condition " + curCond.ToString() + ": ", res);
            }
                
            if (curCond < expressions.Count)
                return expressions[curCond].Step();

            resCond = null;
            curCond = 0;

            return new DoneData();
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
                str += "else" + expressions[i].ToString();

            return str;
        }

        public override bool ContainsVariable(Variable other)
        {
            throw new NotImplementedException();
        }
    }
}

