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

        public override Expression Evaluate()
        {
            return expr.Evaluate();
        }

        public override EvalData Step()
        {
            return new ReturnData(Evaluate());
        }

        public override string ToString()
        {
            return "ret " + expr.ToString();
        }
    }
}

