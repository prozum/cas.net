using System;

namespace Ast
{
    public class ForStmt : Expression
    {
        public Symbol sym;
        public Expression list;

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

        public override bool ContainsVariable(Variable other)
        {
            throw new NotImplementedException();
        }
    }
}

