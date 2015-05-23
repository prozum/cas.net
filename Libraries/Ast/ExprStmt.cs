using System;

namespace Ast
{
    public class ExprStmt : Statement
    {
        public Expression Expression;

        public ExprStmt(Expression expr, Scope scope) : base(scope)
        {
            Expression = expr;
        }

        public override void Evaluate()
        {
            var res = Expression.Evaluate();

            if (CurScope.GetBool("debug"))
                CurScope.SideEffects.Add(new DebugData("Debug: " + Expression + " = " + res));

            if (res is Error)
            {
                CurScope.Errors.Add(new ErrorData(res as Error));
                return;
            }

            if (!(res is Null))
                CurScope.Returns.Add(res);
        }

        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}

