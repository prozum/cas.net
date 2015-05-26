namespace Ast
{
    public abstract class UnaryOperator : Expression
    {
        public Expression Child;

        public override Scope CurScope
        {
            get { return base.CurScope; }
            set
            {
                base.CurScope = value;
                if (Child != null)
                {
                    Child.CurScope = value;
                }
            }
        }

        public override bool ContainsVariable(Variable other)
        {
            return Child.ContainsVariable(other);
        }

        public override bool CompareTo(Expression other)
        {
            if (GetType() == other.GetType() && Child.CompareTo((other as UnaryOperator).Child))
                return true;

            return false;
        }
    }
}

