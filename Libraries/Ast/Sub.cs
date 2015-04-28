using System;

namespace Ast
{
    public class Sub : Operator, ISwappable, IInvertable
    {
        public Sub() : base("-", 30) { }
        public Sub(Expression left, Expression right) : base(left, right, "-", 20) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left - Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Sub(left.Expand(), right.Expand());
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            var newRight = new Mul(new Integer(-1), right).Simplify(this);
            return new Add(left, newRight);
        }

        public override Expression Clone()
        {
            return new Sub(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Add(other, Right);
        }

        public Operator Swap()
        {
            return new Add(new Mul(new Integer(-1), Right), Left);
        }

        public Operator Transform()
        {
            if (Left is Add)
            {
                return new Add((Left as Add).Left, new Sub((Left as Add).Right, Right));
            }
            else if (Right is Add)
            {
                return new Add(new Sub(Left, (Right as Add).Left), (Right as Add).Right);
            }
            else if (Left is Sub)
            {
                return new Sub((Left as Sub).Left, new Sub((Left as Sub).Right, Right));
            }
            else if (Right is Sub)
            {
                return new Sub(new Sub(Left, (Right as Sub).Left), (Right as Sub).Right);
            }
            else
            {
                return this;
            }
        }

        public override Expression CurrectOperator()
        {
            if (Right is INegative && (Right as INegative).IsNegative())
            {
                return new Add(Left.CurrectOperator(), (Right as INegative).ToNegative());
            }

            return new Sub(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

