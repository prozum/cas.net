using System;

namespace Ast
{
    public class RetStmt : Statement
    {
        public Expression expr;

        public RetStmt(Expression expr, Scope scope) : base(scope)
        {
            this.expr = expr;
        }
            
        public override EvalData Evaluate()
        {
            var res = expr.Evaluate();

            if (res is ErrorExpr)
                return new ErrorData(res as ErrorExpr);

            return new ReturnData(res);
        }

        public override string ToString()
        {
            return "ret " + expr.ToString();
        }
    }
}

