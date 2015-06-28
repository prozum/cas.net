namespace Ast
{
    public class BooleanEqual : BinaryOperator
    {
        public override string Identifier { get { return "=="; } }
        public override int Priority { get{ return 20; } }

        public BooleanEqual() { }
        public BooleanEqual(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
        {
            return new Boolean(Left.CompareTo(Right));
        }

        public override Expression Clone(Scope scope)
        {
            return new BooleanEqual(Left.Clone(scope), Right.Clone(scope));
        }

        internal override Expression CurrectOperator()
        {
            return new BooleanEqual(Left.CurrectOperator(), Right.CurrectOperator());
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new BooleanEqual(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new BooleanEqual(left, right);
        }
    }
}

