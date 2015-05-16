using System;

namespace Ast
{
    public class Or : BinaryOperator
    {
        public override string Identifier { get { return "|"; } }
        public override int Priority { get{ return 10; } }

        public Or() { }
        public Or(Expression left, Expression right) : base(left, right) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left | Right;
        }
    }
}

