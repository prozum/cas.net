using System;

namespace Ast
{
    public class WhileStmt : Statement
    {
        public Expression condition;
        public Expression expr;
        readonly int MaxIterations = 10000;

        public WhileStmt(Expression expr, Scope scope) : base(scope)
        {
            this.expr = expr;
        }

        public override EvalData Evaluate()
        {
            int i = 0;

            while (i < MaxIterations)
            {
                var res = condition.Evaluate();

                if (res is Boolean)
                {
                    if (!(res as Boolean).@bool)
                        break;
                }
                else if (res is Error)
                    return new ErrorData(res as Error);

            }
//            var res = expr.Evaluate();
//
//            if (Scope.GetBool("debug"))
//                Scope.SideEffects.Add(new DebugData("Debug: " + expr + " = " + res));
//
//            return new ExprData(res);

            return new DoneData();
        }

        public override string ToString()
        {
            return expr.ToString();
        }
    }
}

