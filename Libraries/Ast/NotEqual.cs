using System;

namespace Ast
{
    public class NotEqual : BinaryOperator
    {
        public NotEqual() : base("!=", 10) { }
        public NotEqual(Expression left, Expression right) : base(left, right, "==", 10) { }

        protected override Expression Evaluate(Expression caller)
        {
            return new Boolean(!Left.CompareTo(Right));
        }

        public override Expression Clone()
        {
            return new NotEqual(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new NotEqual(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new NotEqual(left.Reduce(this), right.Reduce(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new NotEqual(left.Expand(), right.Expand());
        }
    }
}

