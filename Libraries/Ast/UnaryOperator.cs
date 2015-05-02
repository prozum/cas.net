using System;

namespace Ast
{
    public abstract class UnaryOperator : Operator
    {
        public string sym;
        public Expression child;

        public UnaryOperator(string sym)
        {
            this.sym = sym;
        }
            
        public override bool ContainsVariable(Variable other)
        {
            return child.ContainsVariable(other);
        }

        public override string ToString()
        {
            return sym + child.ToString();
        }
    }
}

