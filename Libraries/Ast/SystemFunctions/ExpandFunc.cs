using System;
using System.Collections.Generic;

namespace Ast
{
    public class ExpandFunc : SysFunc
    {
        public ExpandFunc() : this(null, null) { }
        public ExpandFunc(List<Expression> args, Scope scope)
            : base("expand", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        public override Expression Evaluate()
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            return Arguments[0].Expand();
        }

        public override Expression Clone()
        {
            return MakeClone<ExpandFunc>();
        }
    }
}

