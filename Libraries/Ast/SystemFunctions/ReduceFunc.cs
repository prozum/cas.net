using System;
using System.Collections.Generic;

namespace Ast
{
    public class ReduceFunc : SysFunc
    {
        public ReduceFunc() : this(null, null) { }
        public ReduceFunc(List<Expression> args, Scope scope)
            : base("reduce", args, scope)
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

            return Arguments[0].Reduce();
        }

        public override Expression Clone()
        {
            return MakeClone<ReduceFunc>();
        }
    }
}

