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

            if (res is ErrorExpr)
                return new ErrorData(res as ErrorExpr);

            return new ExprData(res);
        }

        public override string ToString()
        {
            return expr.ToString();
        }

        public override DebugData GetDebugData()
        {
            return new DebugData("Expression: " + expr.ToString() + " = " + expr.Evaluate());
        }
    }
}

