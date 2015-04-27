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

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((res as Integer).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take ACos of: " + args[0]);
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

