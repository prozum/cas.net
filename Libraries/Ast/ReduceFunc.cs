using System;
using System.Collections.Generic;

namespace Ast
{
    public class ReduceFunc : SysFunc
    {
        public ReduceFunc(List<Expression> args, Scope scope)
            : base("reduce", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            return Arguments[0].Reduce();
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

