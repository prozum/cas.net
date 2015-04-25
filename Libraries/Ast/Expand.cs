using System;
using System.Collections.Generic;

namespace Ast
{
    public class Expand : SysFunc
    {
        public Expand(List<Expression> args, Scope scope)
            : base("expand", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        public override Expression Evaluate()
        {
            return args[0].Expand();
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

