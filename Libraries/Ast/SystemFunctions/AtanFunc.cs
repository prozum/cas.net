using System;
using System.Collections.Generic;

namespace Ast
{
    public class AtanFunc : SysFunc, IInvertable
    {
        public AtanFunc() : this(null, null) { }
        public AtanFunc(List<Expression> args, Scope scope)
            : base("atan", args, scope)
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
                return ReturnValue(new Irrational((decimal)Math.Atan((double)(res as Real)) * (deg ? Constant.RadToDeg.@decimal  : 1) )).Evaluate();
            }

            return new Error(this, "Could not take ATan of: " + Arguments[0]);
        }

        public override Expression Reduce()
        {
            return ReduceHelper<AtanFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<AtanFunc>();
        }

        //atan[x] -> tan[other]
        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new TanFunc(newArgs, CurScope);
        }
    }
}

