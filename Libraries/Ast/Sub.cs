using System;

namespace Ast
{
    public class Sub : BinaryOperator, ISwappable, IInvertable
    {
        public override string Identifier { get { return "-"; } }
        public override int Priority { get{ return 40; } }

        public Sub() { }
        public Sub(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return Left - Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Sub(left.Expand(), right.Expand());
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            var newRight = new Mul(new Integer(-1), right).Reduce(this);
            return new Add(left, newRight);
        }

        public override Expression Clone()
        {
            return new Sub(Left.Clone(), Right.Clone());
        }

        public Expression InvertOn(Expression other)
        {
            return new Add(other, Right);
        }

        public BinaryOperator Swap()
        {
            return new Add(new Mul(new Integer(-1), Right), Left);
        }

        public BinaryOperator Transform()
        {
            if (Left is Add)
            {
                return new Add((Left as Add).Left, new Sub((Left as Add).Right, Right));
            }
            else if (Right is Add)
            {
                return new Add(new Sub(Left, (Right as Add).Left), (Right as Add).Right);
            }
            else
            {
                return this;
            }
        }

        internal override Expression CurrectOperator()
        {
            if (Right is INegative && (Right as INegative).IsNegative())
            {
                return new Add(Left.CurrectOperator(), (Right as INegative).ToNegative());
            }

            return new Sub(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

