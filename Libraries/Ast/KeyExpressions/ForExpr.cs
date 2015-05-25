using System;

namespace Ast
{
    public class ForExpr : Expression
    {
        public string Var;
        public List List;
        public Scope ForScope;

        public ForExpr (Scope scope) 
        { 
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            foreach (var value in List.Items)
            {
                ForScope.SetVar(Var, value);

                var res = ForScope.Evaluate();

                if (CurScope.Error)
                    return Constant.Null;
            }

            return Constant.Null;
        }
    }
}

