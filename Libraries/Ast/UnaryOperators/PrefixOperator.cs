using System;

namespace Ast
{
    public abstract class PrefixOperator : Expression
    {
        public string Identifier;

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

        protected PrefixOperator(string sym)
        {
            this.Identifier = sym;
        }
            
        public override bool ContainsVariable(Variable other)
        {
            return Child.ContainsVariable(other);
        }

        public override string ToString()
        {
            return Identifier + Child.ToString();
        }

        public override bool CompareTo(Expression other)
        {
            return Evaluate().CompareTo(other);
        }
    }
}

