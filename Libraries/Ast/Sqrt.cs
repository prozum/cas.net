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

        protected override Expression Evaluate(Expression caller)
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

        internal override Expression Reduce(Expression caller)
        {
            if (exponent.CompareTo(Constant.Two))
            {
                return args[0];
            }
            else
            {
                var simplified = ReduceHelper<Sqrt>();

                if (simplified.args[0] is Exp && (simplified.args[0] as Exp).Right.CompareTo(Constant.Two))
                {
                    return (simplified.args[0] as Exp).Left;
                }
                else if (simplified.args[0] is Variable && (simplified.args[0] as Variable).exponent.CompareTo(Constant.Two))
                {
                    var res = simplified.args[0].Clone() as Variable;
                    res.exponent = new Integer(1);

                    return res;
                }

                return simplified;
            }
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

