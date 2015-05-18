using System;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class LesserEqual : BinaryOperator
    {
        public override string Identifier { get { return "<="; } }
        public override int Priority { get{ return 20; } }

        public LesserEqual() { }
        public LesserEqual(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return Left <= Right;
        }

        public override Expression Clone()
        {
            return new LesserEqual(Left.Clone(), Right.Clone());
        }

        internal override Expression CurrectOperator()
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

