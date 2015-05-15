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
                return ReturnValue(new Irrational(Math.Acos((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take ACos of: " + Arguments[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<AcosFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<AcosFunc>();
        }

        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new CosFunc(newArgs, Scope);
        }
    }
}

