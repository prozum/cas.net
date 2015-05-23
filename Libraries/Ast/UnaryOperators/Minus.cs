using System;

namespace Ast
{
    public class Minus : UnaryOperator
    {
        public Minus() : base("-") { }

        internal override Expression Evaluate(Expression caller)
        {
            return Child.Evaluate(caller).Minus();
        }
    }
}

