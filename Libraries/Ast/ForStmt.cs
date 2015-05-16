using System;

namespace Ast
{
    public class ForStmt : Statement
    {
        public string sym;
        public List list;
        public Scope expr;

        public ForStmt (Scope scope) : base(scope) { }

        public override EvalData Evaluate()
        {
            foreach (var value in list.items)
            {
                expr.SetVar(sym, value);
                expr.Evaluate();
            }

            return new DoneData();
        }
    }
}

