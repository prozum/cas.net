using System;

namespace Ast
{
    public class Null : Expression
    {
        public Null()
        {
        }

        public override string ToString()
        {
            return "null";
        }

        public override Expression Evaluate()
        {
            return this;
        }
        protected override Expression Evaluate(Expression caller)
        {
            return this;
        }
    }
}

