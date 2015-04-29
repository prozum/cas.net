using System;

namespace Ast
{
    public class FuncStmt : Statement
    {
        public Func func;

        public FuncStmt()
        {
        }

        public override EvalData Step()
        {
            return func.Step();
        }

        public override Expression Evaluate()
        {
            throw new NotSupportedException();
        }
    }
}

