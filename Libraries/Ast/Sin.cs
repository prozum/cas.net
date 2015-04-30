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

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((res as Integer).value * (degrees ? (Math.PI / 180) : 1)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((double)(res as Rational).value.value * (degrees ? (Math.PI / 180) : 1)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((double)(res as Irrational).value * (degrees ? (Math.PI / 180) : 1)))).Evaluate();
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

