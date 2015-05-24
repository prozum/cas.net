using System;
using System.Collections.Generic;

namespace Ast
{
    public class CosFunc : SysFunc, IInvertable
    {
        public CosFunc() : this(null, null) { }
        public CosFunc(List<Expression> args, Scope scope)
            : base("cos", args, scope)
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
                return ReturnValue(new Irrational(Math.Cos((double) ((deg ? Constant.DegToRad.@decimal  : 1) * (res as Real)) ))).Evaluate();
            }

            return new Error(this, "Could not take Cos of: " + Arguments[0]);
        }

        public override Expression Reduce()
        {
            return ReduceHelper<CosFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<CosFunc>();
        }

        //cos[x] -> acos[other]
        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new AcosFunc(newArgs, CurScope);
        }
    }
}

