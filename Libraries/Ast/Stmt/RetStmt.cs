using System;

namespace Ast
{
    public class RetStmt : Expression
    {
        public Expression expr;

        public RetStmt(Expression expr)
        {
            this.expr = expr;
        }

        public override EvalData Step()
        {
            return new ReturnData(expr.Evaluate());
        }

        public override string ToString()
        {
            return "ret " + expr.ToString();
        }
    }
}

