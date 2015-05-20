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

        public override EvalData Evaluate()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                var res = conditions[i].Evaluate();

                if (res is Boolean)
                {
                    if (res as Boolean)
                    {
                        expressions[i].Evaluate();
                        return new DoneData();
                    }

                    continue;
                }
            }

            if (expressions.Count > conditions.Count)
                expressions[expressions.Count - 1].Evaluate();

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
                str += "else:" + expressions[i].ToString();

            return str;
        }
    }
}

