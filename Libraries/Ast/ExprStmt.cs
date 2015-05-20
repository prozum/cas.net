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

        public override EvalData Evaluate()
        {
            var res = expr.Evaluate();

            if (res is Error)
                return new ErrorData(res as Error);

            if (Scope.GetBool("debug"))
                Scope.SideEffects.Add(new DebugData("Debug: " + expr + " = " + res));

            if (!(res is Null))
                return new ExprData(res);
            else
                return new DoneData();
        }

        public override string ToString()
        {
            return expr.ToString();
        }
    }
}

