using System;
using System.Collections.Generic;

namespace Ast
{
    public class AbsFunc : SysFunc
    {
        public AbsFunc() : this(null, null) { }
        public AbsFunc(List<Expression> args, Scope scope)
            : base("abs", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        internal override Expression Evaluate(Expression caller)
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            var res = Arguments[0].Evaluate();

            if (res is INegative)
            {
                return (res as INegative).ToNegative();
            }

            return new Error(this, "Could not take Abs of: " + Arguments[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<AbsFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<AbsFunc>();
        }
    }
}

