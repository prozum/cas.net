using System;
using System.Collections.Generic;

namespace Ast
{
    public class Sin : SysFunc, IInvertable
    {
        public Sin() : this(null, null) { }
        public Sin(List<Expression> args, Scope scope)
            : base("sin", args, scope)
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
                return ReturnValue(new Irrational(Math.Sin((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take Sin of: " + args[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<Sin>();
        }

        public override Expression Clone()
        {
            return MakeClone<Sin>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new ASin(newArgs, scope);
        }
    }
}

