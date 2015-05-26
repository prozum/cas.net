using System;

namespace Ast
{
    public class WhileExpr : Expression
    {
        public Expression Condition;
        public Expression WhileScope;

        readonly int MaxIterations = 10000;

        public WhileExpr(Scope scope)
        {
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            int i = 0;

            while (i++ < MaxIterations)
            {
                var res = Condition.ReduceEvaluate();

                if (res is Error)
                    return res;

                if (res is Boolean)
                {
                    if (!(res as Boolean).@bool)
                        break;
                }

                WhileScope.Evaluate();
            }

            if (i > MaxIterations)
                return new Error(this, "Overflow");

            return Constant.Null;

        }

        public override string ToString()
        {
            return WhileScope.ToString();
        }
    }
}

