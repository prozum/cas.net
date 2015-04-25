using System;
using System.Collections.Generic;

namespace Ast
{

    public class Enter : SysFunc
    {
        public Enter(List<Expression> args, Scope scope)
            : base("enter", args, scope)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }
    }
}

