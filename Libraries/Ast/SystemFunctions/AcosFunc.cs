using System;
using System.Collections.Generic;

namespace Ast
{
    public class AcosFunc : SysFunc, IInvertable
    {
        public AcosFunc() : this(null, null) { }
        public AcosFunc(List<Expression> args, Scope scope)
            : base("acos", args, scope)
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
                    return ReturnValue(new Irrational((decimal)Math.Acos(value) * (deg ? Constant.RadToDeg.@decimal : 1))).Evaluate();
            }

            return new Error(this, "Could not take ACos of: " + Arguments[0]);
        }

        public override Expression Reduce()
        {
            return ReduceHelper<AcosFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<AcosFunc>();
        }

        //acos[x] -> cos[other]
        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new CosFunc(newArgs, CurScope);
        }
    }
}

