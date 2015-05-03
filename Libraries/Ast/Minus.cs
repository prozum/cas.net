using System;

namespace Ast
{
    public class Minus : UnaryOperator
    {
        public Minus() : base("-") { }

        public override Expression Evaluate()
        {
            return child.Evaluate().Minus();
        }
    }
}

