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
                return ReturnValue(new Irrational(Math.Asin((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take ASin of: " + Arguments[0]);
        }

        internal override Expression Reduce(Expression caller)
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
            return new SinFunc(newArgs, Scope);
        }
    }
}

