using System;

namespace Ast
{
    public class GlobalExpr : Expression
    {
        Expression Expression;

        public GlobalExpr(Expression expr, Scope scope)
        {
            Expression = expr;
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            if (Expression is List)
            {
                foreach (var expr in (Expression as List).Items)
                {
                    if (expr is Error)
                        return expr;

                    if (expr is Variable)
                        CurScope.Globals.Add((expr as Variable).Identifier);
                    else
                        return new Error(Expression, "contains Non-Variables");
                }
            }

            if (Expression is Variable)
                CurScope.Globals.Add((Expression as Variable).Identifier);
            else
                return new Error(Expression, "is not at Variable");

            return Constant.Null;
        }

        public override Expression Clone(Scope scope)
        {
            return new GlobalExpr(Expression.Clone(scope), scope);
        }

        public override string ToString()
        {
            return "global " + Expression.ToString();
        }
    }
}

