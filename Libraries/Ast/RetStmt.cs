using System;

namespace Ast
{
    public class RetStmt : Statement
    {
        public Expression Expression;

        public RetStmt(Expression expr, Scope scope) : base(scope)
        {
            Expression = expr;
        }
            
        public override void Evaluate()
        {
            var res = Expression.Evaluate();

            if (res is Error)
            {
                CurScope.Errors.Add(new ErrorData(res as Error));
                return;
            }

            CurScope.Returns.Clear();
            CurScope.Returns.Add(res);
            CurScope.Return = true;
        }

        public override string ToString()
        {
            return "ret " + Expression.ToString();
        }
    }
}

