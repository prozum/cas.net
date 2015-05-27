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
                return res;

            if (res is List)
            {
                foreach (var expr in (res as List).Items)
                {
                    res = expr.Value;

                    if (res is Error)
                        return res;

                    if (res is VarFunc)
                        res = Expression.Evaluate();

                    if (res is Error)
                        return res;

                    if (res is Scope)
                        ImportScope(res as Scope);
                    else
                        return new Error(Expression, "contains Non-Scope");
                }
            }

            if (res is VarFunc)
                res = Expression.Evaluate();

            if (res is Error)
                return res;

            if (res is Scope)
                ImportScope(res as Scope);
            else
                return new Error(Expression, "contains Non-Scope");

            return Constant.Null;
        }

        public void ImportScope(Scope scope)
        {
            foreach (var @var in scope.Locals)
            {
                CurScope.SetVar(@var.Key, @var.Value);
            }
        }

        public override string ToString()
        {
            return "import " + Expression.ToString();
        }
    }
}

