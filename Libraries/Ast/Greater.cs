using System;

namespace Ast
{
    public class Greater : Operator
    {
        public Greater() : base(">", 10) { }
        public Greater(Expression left, Expression right) : base(left, right, ">", 10) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left > Right;
        }

        public override Expression Clone()
        {
            return new Greater(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new Greater(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Greater(left.Reduce(this), right.Reduce(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Greater(left.Expand(), right.Expand());
        }
    }
}

