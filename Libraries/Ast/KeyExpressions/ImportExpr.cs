﻿using System;

namespace Ast
{
    public class ImportExpr : Expression
    {
        public Expression Expression;

        public ImportExpr(Expression expr, Scope scope)
        {
            Expression = expr;
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            var res = Expression.Value;


            if (res is Error)
            {
                CurScope.Errors.Add(new ErrorData(res as Error));
                return new Null();
            }

            if (res is Scope)
            {
                Scope scope = (Scope)res;

                foreach (var @var in scope.Locals)
                {
                    CurScope.SetVar(@var.Key, @var.Value);
                }
            }
            else
            {
                CurScope.Errors.Add(new ErrorData(Expression, "is not a Scope"));
            }

            return new Null();
        }

        public override string ToString()
        {
            return "import " + Expression.ToString();
        }
    }
}

