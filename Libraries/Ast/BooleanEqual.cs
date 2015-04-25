using System;

namespace Ast
{
    public class BooleanEqual : Operator
    {
        public BooleanEqual() : base("==", 10) { }
        public BooleanEqual(Expression left, Expression right) : base(left, right, "==", 10) { }

        public override Expression Evaluate()
        {
            return new Boolean(Left.CompareTo(Right));
        }

        public override Expression Clone()
        {
            return new BooleanEqual(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new BooleanEqual(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            return new BooleanEqual(left.Simplify(), right.Simplify());
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new BooleanEqual(left.Expand(), right.Expand());
        }
    }
}

