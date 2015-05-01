using System;
using System.Collections.Generic;

namespace Ast
{
    public class SqrtFunc : SysFunc, IInvertable
    {
        public SqrtFunc() : this(null, null) { }
        public SqrtFunc(List<Expression> args, Scope scope)
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

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Sqrt((double)(res as Real).Value))).Evaluate();
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
                var simplified = ReduceHelper<SqrtFunc>();

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
            return MakeClone<SqrtFunc>();
        }

        public Expression Inverted(Expression other)
        {
            return new Exp(other, new Integer(2));
        }
    }
}

