using System;

namespace Ast
{
    public class NotEqual : BinaryOperator
    {
        public override string Identifier { get { return "!="; } }
        public override int Priority { get{ return 20; } }

        public NotEqual() { }
        public NotEqual(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return new Boolean(!Left.CompareTo(Right));
        }

        public override Expression Clone()
        {
            return new NotEqual(Left.Clone(), Right.Clone());
        }

        internal override Expression CurrectOperator()
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

