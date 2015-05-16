using System;

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
    }
}

