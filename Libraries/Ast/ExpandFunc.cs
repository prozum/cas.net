using System;
using System.Collections.Generic;

namespace Ast
{
    public class ExpandFunc : SysFunc
    {
        public ExpandFunc(List<Expression> args, Scope scope)
            : base("expand", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            return args[0].Expand();
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

