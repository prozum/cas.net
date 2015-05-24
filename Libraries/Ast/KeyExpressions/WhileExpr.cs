using System;

namespace Ast
{
    public class WhileExpr : Expression
    {
        public Expression Condition;
        public Expression Expression;

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

                if (res is Boolean)
                {
                    if (!(res as Boolean).@bool)
                        break;
                }
                else if (res is Error)
                {
                    CurScope.Errors.Add(new ErrorData(res as Error));
                    return new Null();
                }

                Expression.Evaluate();
            }

            if (i > MaxIterations)
                CurScope.Errors.Add(new ErrorData("while: Overflow!"));

            return new Null();

        }

        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}

