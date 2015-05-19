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

            var res = ReturnValue(Arguments[0]).Evaluate();

            if (res is INegative)
            {
                if ((res as INegative).IsNegative())
                    return (res as INegative).ToNegative();
                else
                    return res;
            }

            if (res is Complex)
            {
                var c = res as Complex;

                return new Irrational(Math.Sqrt(Math.Pow((double)Math.Abs(c.real.@decimal),2) + Math.Pow((double)Math.Abs(c.imag.@decimal),2)));
                                        
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

