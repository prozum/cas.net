using System;

namespace Ast
{
    public class ForStmt : Statement
    {
        public string Var;
        public List List;
        public Scope ForScope;

        public ForStmt (Scope scope) : base(scope) { }

        public override void Evaluate()
        {
            foreach (var value in List.items)
            {
                ForScope.SetVar(Var, value);
                var res = ForScope.Evaluate();

                if (res is Error)
                {
                    CurScope.Errors.Add(new ErrorData(res as Error));
                    return;
                }
            }
        }
    }
}

