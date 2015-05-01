using System;
using System.Collections.Generic;

namespace Ast
{

    public class EnterFunc : SysFunc
    {
        public EnterFunc(List<Expression> args, Scope scope)
            : base("enter", args, scope)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }
    }
}

