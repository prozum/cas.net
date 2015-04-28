using System;
using System.Collections.Generic;

namespace Ast
{
    public class Negation : UnaryOperator
    {
        public Negation() : base("!") { }

        protected override Expression Evaluate(Expression caller)
        {
            return child.Evaluate().Negation();
        }
    }
}

