using System;

namespace Ast
{
    public class And : BinaryOperator
    {
        public override string Identifier { get { return "&"; } }
        public override int Priority { get{ return 10; } }

        public And() { }
        public And(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
        {
            return Left & Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new And(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new And(left, right);
        }
    }
}

