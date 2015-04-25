using System;
using System.Collections.Generic;

namespace Ast
{
    public class Exit : SysFunc
    {
        public Exit(List<Expression> args, Scope scope)
            : base("exit", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }
    }
}

