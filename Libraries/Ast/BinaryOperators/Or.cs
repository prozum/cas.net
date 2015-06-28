namespace Ast
{
    public class Or : BinaryOperator
    {
        public override string Identifier { get { return "|"; } }
        public override int Priority { get{ return 10; } }

        public Or() { }
        public Or(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
        {
            return Left | Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Or(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Or(left, right);
        }

        public override Expression Clone(Scope scope)
        {
            return new Or(Left.Clone(scope), Right.Clone(scope));
        }
    }
}

