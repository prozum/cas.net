using System;

namespace Ast
{
    public class ForStmt : Expression
    {
        public string sym;
        public List list;
        public Scope expr;

        private int curItem = 0;

        public override Expression Evaluate()
        {
            throw new NotImplementedException();
        }

        public override EvalData Step()
        {
            EvalData res;

            while (curItem < list.items.Count)
            {
                expr.SetVar(sym, list.items[curItem]);
                res = expr.Step();

                if (res is ReturnData || res is ErrorData)
                    Reset();

                if (res is DoneData)
                    curItem++;
                else
                    return res;
            }

            Reset();
            return new DoneData();
        }

        public void Reset()
        {
            curItem = 0;
        }

        public override bool ContainsVariable(Variable other)
        {
            throw new NotImplementedException();
        }
    }
}

