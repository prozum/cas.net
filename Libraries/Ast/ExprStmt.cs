using System;

namespace Ast
{
    public class ExprStmt : Statement
    {
        public Expression expr;

        public ExprStmt(Expression expr, Scope scope) : base(scope)
        {
            this.expr = expr;
        }

        public override void Evaluate()
        {
            var res = expr.Evaluate();

            if (Scope.GetBool("debug"))
                Scope.SideEffects.Add(new DebugData("Debug: " + expr + " = " + res));

            if (res is Error)
            {
                Scope.Errors.Add(new ErrorData(res as Error));
                return;
            }

            if (!(res is Null))
                Scope.Returns.Add(res);
        }

        public override string ToString()
        {
            return expr.ToString();
        }
    }
}

