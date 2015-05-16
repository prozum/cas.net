using System;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class Lesser : BinaryOperator
    {
        public override string Identifier { get { return "<"; } }
        public override int Priority { get{ return 20; } }

        public Lesser() { }
        public Lesser(Expression left, Expression right) : base(left, right) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left < Right;
        }

        public override Expression Clone()
        {
            return new Lesser(Left.Clone(), Right.Clone());
        }

        internal override Expression CurrectOperator()
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

