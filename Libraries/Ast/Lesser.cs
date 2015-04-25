using System;

namespace Ast
{
    public class Lesser : Operator
    {
        public Lesser() : base("<", 10) { }
        public Lesser(Expression left, Expression right) : base(left, right, "<", 10) { }

        public override Expression Evaluate()
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

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            return new Lesser(left.Simplify(), right.Simplify());
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Lesser(left.Expand(), right.Expand());
        }
    }
}

