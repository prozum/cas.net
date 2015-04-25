using System;
using System.Collections.Generic;

namespace Ast
{
    public class Sqrt : SysFunc, IInvertable
    {
        public Sqrt() : this(null, null) { }
        public Sqrt(List<Expression> args, Scope scope)
            : base("sqrt", args, scope)
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

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Sqrt((res as Integer).value))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sqrt((double)(res as Rational).value.value))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sqrt((double)(res as Irrational).value))).Evaluate();
            }

            return new Error(this, "Could not take Sqrt of: " + args[0]);
        }

        public override Expression Simplify()
        {
            if (exponent.CompareTo(Constant.Two))
            {
                return args[0];
            }

            return base.Simplify();
        }

        public override Expression Clone()
        {
            return MakeClone<Sqrt>();
        }

        public Expression Inverted(Expression other)
        {
            return new Exp(other, new Integer(2));
        }
    }
}

