using System;

namespace Ast
{
    public class BooleanEqual : BinaryOperator
    {
        public override string Identifier { get { return "=="; } }
        public override int Priority { get{ return 20; } }

        public BooleanEqual() { }
        public BooleanEqual(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return new Boolean(Left.CompareTo(Right));
        }

        public override Expression Clone()
        {
            return new BooleanEqual(Left.Clone(), Right.Clone());
        }

        internal override Expression CurrectOperator()
        {
            return new BooleanEqual(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new BooleanEqual(left.Reduce(this), right.Reduce(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new BooleanEqual(left.Expand(), right.Expand());
        }
    }
}

