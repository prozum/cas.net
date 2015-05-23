using System;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class GreaterEqual : BinaryOperator
    {
        public override string Identifier { get { return ">="; } }
        public override int Priority { get{ return 20; } }

        public GreaterEqual() { }
        public GreaterEqual(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return Left >= Right;
        }

        public override Expression Clone()
        {
            return new GreaterEqual(Left.Clone(), Right.Clone());
        }

        internal override Expression CurrectOperator()
        {
            return new GreaterEqual(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new GreaterEqual(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new GreaterEqual(left, right);
        }
    }
}

