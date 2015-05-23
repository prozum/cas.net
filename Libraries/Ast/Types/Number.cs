using System;

namespace Ast
{
    public abstract class Number : Expression
    {
        public override Expression Evaluate()
        {
            return this;
        }

        internal override Expression Evaluate(Expression caller)
        {
            return this;
        }
    }
}

