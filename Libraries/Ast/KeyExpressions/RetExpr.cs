﻿namespace Ast
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
                return res;

            CurScope.Returns.Clear();
            CurScope.Returns.Add(res);
            CurScope.Return.@bool = true;

            return res;
        }

        public override string ToString()
        {
            return "ret " + Expression.ToString();
        }
    }
}

