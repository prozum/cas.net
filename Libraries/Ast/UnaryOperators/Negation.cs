using System;
using System.Collections.Generic;

namespace Ast
{
    public class Negation : UnaryOperator
    {
        public Negation() : base("!") { }

        internal override Expression Evaluate(Expression caller)
        {
            return Child.Evaluate().Negation();
        }
    }
}

