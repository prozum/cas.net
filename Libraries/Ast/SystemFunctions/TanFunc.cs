using System;
using System.Collections.Generic;

namespace Ast
{
    public class TanFunc : SysFunc, IInvertable
    {
        public TanFunc() : this(null, null) { }
        public TanFunc(List<Expression> args, Scope scope)
            : base("tan", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Real
                };
        }

        public override Expression Evaluate()
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            var res = Arguments[0].Evaluate();

            var deg = GetBool("deg");

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Tan((double) ((deg ? Constant.DegToRad.@decimal  : 1) * (res as Real)) ))).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + Arguments[0]);
        }

        public override Expression Reduce()
        {
            return ReduceHelper<TanFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<TanFunc>();
        }

        // tan[x] -> atan[other]
        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new AtanFunc(newArgs, CurScope);
        }
    }
}

