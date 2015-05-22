using System;

namespace Ast
{
    public class ForStmt : Statement
    {
        public string sym;
        public List list;
        public Scope expr;

        public ForStmt (Scope scope) : base(scope) { }

        public override void Evaluate()
        {
            foreach (var value in list.items)
            {
                expr.SetVar(sym, value);
                var res = expr.Evaluate();

                if (res is Error)
                {
                    Scope.Errors.Add(new ErrorData(res as Error));
                    return;
                }
            }
        }
    }
}

