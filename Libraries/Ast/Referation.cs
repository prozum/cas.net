using System;

namespace Ast
{
    public class Referation : UnaryOperator
    {
        public Referation() : base("~") { }

        public override Expression Value
        {
            get
            {
                return Child.Value;
            }
            set
            {
                Child.Value = value;
            }
        }

        internal override Expression Evaluate(Expression caller)
        {
            return Child.Value;
        }


    }
}

