using System;

namespace Ast
{
    public abstract class Statement
    {
        public virtual Scope CurScope { get; set; }

        public Pos Position;

        public Statement(Scope scope)
        {
            CurScope = scope;
        }

        public abstract void Evaluate();
    }
}

