using System;
using System.Collections.Generic;

namespace Ast
{
    public class Reduce : SysFunc
    {
        public Reduce(List<Expression> args, Scope scope)
            : base("reduce", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            if (!isArgsValid())
                return new ArgError(this);

            return args[0].Reduce();
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

