using System;

namespace Ast
{
    public class Minus : PrefixOperator
    {
        public Minus() : base("-") { }

        public override Expression Evaluate()
        {
            return Child.Evaluate().Minus();
        }
    }
}

