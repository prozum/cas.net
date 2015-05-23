using System;
using System.Collections.Generic;

namespace Ast
{
    public class AsinFunc : SysFunc, IInvertable
    {
        public AsinFunc() : this(null, null) { }
        public AsinFunc(List<Expression> args, Scope scope)
            : base("asin", args, scope)
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
                double value = res as Real;

                if (value >= -1 && value <= 1)
                    return ReturnValue(new Irrational((decimal)Math.Asin(value) * (deg ? Constant.RadToDeg.@decimal  : 1))).Evaluate();
            }

            return new Error(this, "Could not take ASin of: " + Arguments[0]);
        }

        public override Expression Reduce()
        {
            return ReduceHelper<AsinFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<AsinFunc>();
        }

        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new SinFunc(newArgs, CurScope);
        }
    }
}

