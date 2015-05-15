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
                return ReturnValue(new Irrational(Math.Cos((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take Cos of: " + Arguments[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<CosFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<CosFunc>();
        }

        public Expression InvertOn(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new AcosFunc(newArgs, Scope);
        }
    }
}

