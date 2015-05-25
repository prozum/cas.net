using System;

namespace Ast
{
    public class NotEqual : BinaryOperator
    {
        public override string Identifier { get { return "!="; } }
        public override int Priority { get{ return 20; } }

        public NotEqual() { }
        public NotEqual(Expression left, Expression right, Scope scope) : base(left, right, scope) { }

        public override Expression Evaluate()
        {
            return new Boolean(!Left.CompareTo(Right));
        }

        public override Expression Clone()
        {
            return new NotEqual(Left.Clone(), Right.Clone(), CurScope);
        }

        internal override Expression CurrectOperator()
        {
            return new NotEqual(Left.CurrectOperator(), Right.CurrectOperator(), CurScope);
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new NotEqual(left, right, CurScope);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new NotEqual(left, right, CurScope);
        }
    }
}

