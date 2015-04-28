using System;

namespace Ast
{
    public class ExprStmt : Statement
    {
        public Expression expr;

        public ExprStmt(Expression expr)
        {
            this.expr = expr;
        }

        public override EvalData Step()
        {
            return expr.Step();
        }

        public override Expression Evaluate()
        {
            return expr.Evaluate();
        }

        public override string ToString()
        {
            return expr.ToString();
        }

    }
}

