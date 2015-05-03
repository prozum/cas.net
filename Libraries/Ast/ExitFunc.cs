using System;
using System.Collections.Generic;

namespace Ast
{
    public class ExitFunc : SysFunc
    {
        public ExitFunc(List<Expression> args, Scope scope)
            : base("exit", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }
    }
}

