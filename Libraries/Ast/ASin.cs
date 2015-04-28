using System;
using System.Collections.Generic;

namespace Ast
{
    public class ASin : SysFunc, IInvertable
    {
        public ASin() : this(null, null) { }
        public ASin(List<Expression> args, Scope scope)
            : base("asin", args, scope)
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
                return ReturnValue(new Irrational((decimal)(Math.Asin((res as Integer).value) * Math.Pow((180 / Math.PI), degrees ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), degrees ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), degrees ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take ASin of: " + args[0]);
        }

        public override Expression Clone()
        {
            return MakeClone<ASin>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new Sin(newArgs, scope);
        }
    }
}

