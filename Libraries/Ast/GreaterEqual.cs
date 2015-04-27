using System;

namespace Ast
{
    public class GreaterEqual : Operator
    {
        public GreaterEqual() : base(">=", 10) { }
        public GreaterEqual(Expression left, Expression right) : base(left, right, ">=", 10) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left >= Right;
        }

        public override Expression Clone()
        {
            return new GreaterEqual(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new GreaterEqual(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            return new GreaterEqual(left.Simplify(this), right.Simplify(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new GreaterEqual(left.Expand(), right.Expand());
        }
    }
}

