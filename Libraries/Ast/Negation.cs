using System;
using System.Collections.Generic;

namespace Ast
{
    public class Negation : SysFunc
    {
        public Negation() : this(null, null) { }
        public Negation(List<Expression> args, Scope scope)
            : base("negation", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            throw new NotImplementedException();
        }

        public override Expression Clone()
        {
            return MakeClone<Negation>();
        }
    }
}

