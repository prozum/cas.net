using System;

namespace Ast
{
    public class Mod : BinaryOperator
    {
        public override string Identifier { get { return "%"; } }
        public override int Priority { get{ return 40; } }

        public Mod() { }
        public Mod(Expression left, Expression right, Scope scope) : base(left, right, scope) { }

        public override Expression Evaluate()
        {
            return Left % Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Mod(left, right, CurScope);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Mod(left, right, CurScope);
        }
    }
}

