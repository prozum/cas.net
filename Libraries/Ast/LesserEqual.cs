using System;

namespace Ast
{
    public class LesserEqual : BinaryOperator
    {
        public LesserEqual() : base("<=", 10) { }
        public LesserEqual(Expression left, Expression right) : base(left, right, "<=", 10) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left <= Right;
        }

        public override Expression Clone()
        {
            return new LesserEqual(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new LesserEqual(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new LesserEqual(left.Reduce(this), right.Reduce(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new LesserEqual(left.Expand(), right.Expand());
        }
    }
}

