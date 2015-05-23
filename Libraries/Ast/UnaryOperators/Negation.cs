using System;
using System.Collections.Generic;

namespace Ast
{
    public class Negation : UnaryOperator
    {
        public Negation() : base("!") { }

        public override Expression Evaluate()
        {
            return Child.Evaluate().Negation();
        }
    }
}

