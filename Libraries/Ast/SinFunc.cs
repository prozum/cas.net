using System;
using System.Collections.Generic;

namespace Ast
{
    public class SinFunc : SysFunc, IInvertable
    {
        public SinFunc() : this(null, null) { }
        public SinFunc(List<Expression> args, Scope scope)
            : base("sin", args, scope)
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

            var res = Arguments[0].Evaluate();

            var deg = Scope.GetBool("deg");

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Sin((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take Sin of: " + Arguments[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<SinFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<SinFunc>();
        }

        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new AsinFunc(newArgs, Scope);
        }
    }
}

