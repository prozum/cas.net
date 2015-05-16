using System;

namespace Ast
{
    public abstract class Statement
    {
        public Scope Scope;
        public Pos Position;

        public Statement(Scope scope)
        {
            Scope = scope;
        }

        public abstract EvalData Evaluate();
    }
}

