using System;

namespace Ast
{
    public class LesserEqual : Operator
    {
        public LesserEqual() : base("<=", 10) { }
        public LesserEqual(Expression left, Expression right) : base(left, right, "<=", 10) { }

        public override Expression Evaluate()
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

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            return new LesserEqual(left.Simplify(), right.Simplify());
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new LesserEqual(left.Expand(), right.Expand());
        }
    }
}

