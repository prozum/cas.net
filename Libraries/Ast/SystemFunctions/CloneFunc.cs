using System;
using System.Collections.Generic;

namespace Ast
{
    public class CloneFunc : SysFunc
    {
        public CloneFunc() : this(null) { }
        public CloneFunc(Scope scope) : base("clone", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Expression
                };
        }

        public override Expression Call(List args)
        {
            return args[0].Clone();
        }
    }
}

