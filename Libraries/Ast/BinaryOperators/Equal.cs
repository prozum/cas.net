using System;

namespace Ast
{
    public class Equal : BinaryOperator
    {
        public override string Identifier { get { return "="; } }
        public override int Priority { get{ return 0; } }

        public Equal() { }
        public Equal(Expression left, Expression right, Scope scope) : base(left, right, scope) { }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Equal(left, right, CurScope);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Equal(left, right, CurScope);
        }

        public override Expression Clone()
        {
            return new Equal(Left.Clone(), Right.Clone(), CurScope);
        }

        internal override Expression CurrectOperator()
        {
            return new Equal(Left.CurrectOperator(), Right.CurrectOperator(), CurScope);
        }
    }
}

