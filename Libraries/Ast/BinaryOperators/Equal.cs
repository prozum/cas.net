namespace Ast
{
    public class Equal : BinaryOperator
    {
        public override string Identifier { get { return "="; } }
        public override int Priority { get{ return 0; } }

        public Equal() { }
        public Equal(Expression left, Expression right) : base(left, right) { }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Equal(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Equal(left, right);
        }

        public override Expression Clone(Scope scope)
        {
            return new Equal(Left.Clone(scope), Right.Clone(scope));
        }

        internal override Expression CurrectOperator()
        {
            return new Equal(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

