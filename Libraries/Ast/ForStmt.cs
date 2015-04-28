using System;

namespace Ast
{
    public class ForStmt : Statement
    {
        public Symbol sym;
        public Expression list;

        public Scope scope;

        public ForStmt()
        {
        }

        public override Expression Evaluate()
        {
            throw new NotImplementedException();
        }

        public override EvalData Step()
        {
            throw new NotImplementedException();
        }
    }
}

