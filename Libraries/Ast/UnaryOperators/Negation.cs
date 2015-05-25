using System;
using System.Collections.Generic;

namespace Ast
{
    public class Negation : PrefixOperator
    {
        public Negation() : base("!") { }

        public override Expression Evaluate()
        {
            return Child.Evaluate().Negation();
        }

        public override Expression Reduce()
        {
            var res = new Negation();
            res.Child = Child.Reduce();
            return res;
        }
    }
}

