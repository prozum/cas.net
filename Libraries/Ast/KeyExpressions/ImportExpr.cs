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

            if (res is VarFunc)
                res = Expression.Evaluate();
           
            if (res is Error)
                return res;

            if (res is Scope)
            {
                Scope scope = (Scope)res;

                foreach (var @var in scope.Locals)
                {
                    CurScope.SetVar(@var.Key, @var.Value);
                }
            }
            else
                return new Error(Expression, "is not a Scope");

            return Constant.Null;
        }

        public override string ToString()
        {
            return "import " + Expression.ToString();
        }
    }
}

