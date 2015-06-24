namespace Ast
{
    public class ForExpr : Expression
    {
        public string Var;
        public Expression List;
        public Expression ForScope;

        public ForExpr (Scope scope) 
        { 
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            var list = List.Evaluate();
            var resList = new List();

            if (list is Error)
                return list;

            if (!(list is List))
                return new Error(this, list.ToString() + " is not a list");

            foreach (var value in (list as List).Items)
            {
                (ForScope as Scope).SetVar(Var, value);
                var res = (ForScope as Scope).Evaluate();

                if (res is Error)
                    return res;

                resList.Items.Add(res);
            }

            return resList;
        }

        public override string ToString()
        {
            return "for " + Var + " in " + List.ToString() + ":" + ForScope.ToString();
        }
    }
}

