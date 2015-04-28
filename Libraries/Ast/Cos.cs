using System;
using System.Collections.Generic;

namespace Ast
{
    public class Cos : SysFunc, IInvertable
    {
        public Cos() : this(null, null) { }
        public Cos(List<Expression> args, Scope scope)
            : base("cos", args, scope)
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
                return ReturnValue(new Irrational((decimal)Math.Cos((res as Integer).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take Cos of: " + args[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<Cos>();
        }

        public override Expression Clone()
        {
            return MakeClone<Cos>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new ACos(newArgs, scope);
        }
    }
}

