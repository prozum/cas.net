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
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var deg = Scope.GetBool("deg");

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Tan((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + args[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<TanFunc>();
        }

        public override Expression Clone()
        {
            return MakeClone<TanFunc>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new AtanFunc(newArgs, Scope);
        }
    }
}

