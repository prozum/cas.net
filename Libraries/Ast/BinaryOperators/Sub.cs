using System;

namespace Ast
{
    public class Sub : BinaryOperator, ISwappable, IInvertable
    {
        public override string Identifier { get { return "-"; } }
        public override int Priority { get{ return 30; } }

        public Sub() { }
        public Sub(Expression left, Expression right, Scope scope) : base(left, right, scope) { }

        public override Expression Evaluate()
        {
            return Left - Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Sub(left.Expand(), right.Expand(), CurScope);
        }

        //Returns the Add version of the Sub. This is done, so Sub doesn't need to implement rules itself.
        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            var newRight = new Mul(new Integer(-1), right, CurScope).Reduce();
            return new Add(left, newRight, CurScope);
        }

        public override Expression Clone()
        {
            return new Sub(Left.Clone(), Right.Clone(), CurScope);
        }

        public Expression InvertOn(Expression other)
        {
            return new Add(other, Right, CurScope);
        }

        public BinaryOperator Swap()
        {
            return new Add(new Mul(new Integer(-1), Right, CurScope), Left, CurScope);
        }

        public BinaryOperator Transform()
        {
            return this;
        }

        internal override Expression CurrectOperator()
        {
            //When right is negative, return a Add, with right as positive. 2-(-2) -> 2+2.
            if (Right is INegative && (Right as INegative).IsNegative())
            {
                return new Add(Left.CurrectOperator(), (Right as INegative).ToNegative(), CurScope);
            }

            return new Sub(Left.CurrectOperator(), Right.CurrectOperator(), CurScope);
        }
    }
}

