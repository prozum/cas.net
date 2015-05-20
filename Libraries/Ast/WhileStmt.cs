using System;

namespace Ast
{
    public class WhileStmt : Statement
    {
        public Expression condition;
        public Expression expression;
        readonly int MaxIterations = 10000;

        public WhileStmt(Scope scope) : base(scope)
        {
        }

        public override EvalData Evaluate()
        {
            int i = 0;

            while (i++ < MaxIterations)
            {
                var res = condition.Evaluate();

                if (res is Boolean)
                {
                    if (!(res as Boolean).@bool)
                        break;
                }
                else if (res is Error)
                    return new ErrorData(res as Error);

                expression.Evaluate();
            }

            if (i > MaxIterations)
                return new ErrorData("while: Overflow!");

            return new DoneData();
        }

        public override string ToString()
        {
            return expression.ToString();
        }
    }
}

