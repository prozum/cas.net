using System;

namespace Ast
{
    public class Mod : BinaryOperator
    {
        public override string Identifier { get { return "%"; } }
        public override int Priority { get{ return 40; } }

        public Mod() { }
        public Mod(Expression left, Expression right) : base(left, right) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left % Right;
        }
    }
}

