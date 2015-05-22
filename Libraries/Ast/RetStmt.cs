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
            
        public override void Evaluate()
        {
            var res = expr.Evaluate();

            if (res is Error)
            {
                Scope.Errors.Add(new ErrorData(res as Error));
                return;
            }

            Scope.Returns.Add(res);
        }

        public override string ToString()
        {
            return "ret " + expr.ToString();
        }
    }
}

