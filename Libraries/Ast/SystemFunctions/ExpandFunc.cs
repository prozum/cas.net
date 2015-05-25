using System;
using System.Collections.Generic;

namespace Ast
{
    public class ExpandFunc : SysFunc
    {
        public ExpandFunc() : this(null) { }
        public ExpandFunc(Scope scope)
            : base("expand", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Expression
                };
        }

        public override Expression Call(List args)
        {
            return args[0].Expand();
                return new ArgumentError(this);

        }
    }
}

