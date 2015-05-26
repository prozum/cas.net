namespace Ast
{
    public class Lesser : BinaryOperator
    {
        public override string Identifier { get { return "<"; } }
        public override int Priority { get{ return 20; } }

        public Lesser() { }
        public Lesser(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
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

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Lesser(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Lesser(left, right);
        }
    }
}

