using System;

namespace Ast
{
    public class RetExpr : Expression
    {
        public Expression Expression;

        public RetExpr(Expression expr, Scope scope)
        {
            Expression = expr;
            CurScope = scope;
        }
            
        public override Expression Evaluate()
        {
            var res = Expression.Evaluate();

            if (res is Error)
            {
                CurScope.Errors.Add(new ErrorData(res as Error));
                return new Null();
            }

            CurScope.Returns.Clear();
            CurScope.Returns.Add(res);
            CurScope.Return = true;

            return new Null();
        }

        public override string ToString()
        {
            return "ret " + Expression.ToString();
        }
    }
}

