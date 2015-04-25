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

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((res as Integer).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + args[0]);
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

