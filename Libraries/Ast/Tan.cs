using System;
using System.Collections.Generic;

namespace Ast
{
    public class Tan : SysFunc, IInvertable
    {
        public Tan() : this(null, null) { }
        public Tan(List<Expression> args, Scope scope)
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

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Tan((double) ((degrees ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + args[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<Tan>();
        }

        public override Expression Clone()
        {
            return MakeClone<Tan>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new ATan(newArgs, scope);
        }
    }
}

