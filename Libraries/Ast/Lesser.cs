using System;

namespace Ast
{
    public class Lesser : Operator
    {
        public Lesser() : base("<", 10) { }
        public Lesser(Expression left, Expression right) : base(left, right, "<", 10) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left < Right;
        }

        public override Expression Clone()
        {
            return new Lesser(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new Lesser(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Lesser(left.Reduce(this), right.Reduce(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Lesser(left.Expand(), right.Expand());
        }
    }
}

