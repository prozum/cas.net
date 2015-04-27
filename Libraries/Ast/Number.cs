using System;

namespace Ast
{
    public abstract class Number : Expression
    {
        protected override Expression Evaluate(Expression caller)
        {
            return this;
        }

        public override bool ContainsVariable(Variable other)
        {
            return false;
        }

        public abstract bool IsNegative();

        public abstract Number ToNegative();
    }






}

