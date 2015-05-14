using System;

namespace Ast
{
    public class ForStmt : Statement
    {
        public string sym;
        public List list;
        public Scope expr;

        private int curItem = 0;

        public ForStmt (Scope scope) : base(scope) { }

        public override EvalData Evaluate()
        {
            EvalData res;

            while (curItem < list.items.Count)
            {
                expr.SetVar(sym, list.items[curItem]);
                //res = expr.Evaluate();

//                if (res is ReturnData || res is ErrorData)
//                    Reset();

//                if (res is DoneData)
//                    curItem++;
//                else
//                    return res;
            }

            Reset();
            return new DoneData();
        }

        public void Reset()
        {
            curItem = 0;
        }
    }
}

