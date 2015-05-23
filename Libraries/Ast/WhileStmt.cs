using System;

namespace Ast
{
    public class WhileStmt : Statement
    {
        public Expression Condition;
        public Expression Expression;

        readonly int MaxIterations = 10000;

        public WhileStmt(Scope scope) : base(scope)
        {
        }

        public override void Evaluate()
        {
            int i = 0;

            while (i++ < MaxIterations)
            {
                var res = Condition.Evaluate();

                if (res is Boolean)
                {
                    if (!(res as Boolean).@bool)
                        break;
                }
                else if (res is Error)
                {
                    CurScope.Errors.Add(new ErrorData(res as Error));
                }

                Expression.Evaluate();
            }

            if (i > MaxIterations)
                CurScope.Errors.Add(new ErrorData("while: Overflow!"));

        }

        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}

