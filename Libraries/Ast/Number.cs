using System;

namespace Ast
{
    public abstract class Number : Expression 
    {
        public override Expression Evaluate()
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

