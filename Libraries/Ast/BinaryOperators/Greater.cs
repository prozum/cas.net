using System;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class Greater : BinaryOperator
    {
        public override string Identifier { get { return ">"; } }
        public override int Priority { get{ return 20; } }

        public Greater() { }
        public Greater(Expression left, Expression right, Scope scope) : base(left, right, scope) { }

        public override Expression Evaluate()
        {
            return Left > Right;
        }

        public override Expression Clone()
        {
            return new Greater(Left.Clone(), Right.Clone(), CurScope);
        }

        internal override Expression CurrectOperator()
        {
            return new Greater(Left.CurrectOperator(), Right.CurrectOperator(), CurScope);
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Greater(left, right, CurScope);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Greater(left, right, CurScope);
        }
    }
}

