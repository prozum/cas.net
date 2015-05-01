using System;
using System.Collections.Generic;

namespace Ast
{
    public class ACos : SysFunc, IInvertable
    {
        public ACos() : this(null, null) { }
        public ACos(List<Expression> args, Scope scope)
            : base("acos", args, scope)
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

            var deg = scope.GetBool("deg");

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Acos((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take ACos of: " + args[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<ACos>();
        }

        public override Expression Clone()
        {
            return MakeClone<ACos>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new Cos(newArgs, scope);
        }
    }
}

