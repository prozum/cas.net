using System;

namespace Ast
{
    public abstract class UnaryOperator : Operator
    {
        public string identifier;

        private Expression _child;
        public Expression Child
        {
            get
            {
                return _child;
            }
            set
            {
                _child = value;

                if (_child != null)
                    _child.Parent = this;
            }
        }

        protected UnaryOperator(string sym)
        {
            this.identifier = sym;
        }
            
        public override bool ContainsVariable(Variable other)
        {
            return Child.ContainsVariable(other);
        }

        public override string ToString()
        {
            return identifier + Child.ToString();
        }
    }
}

