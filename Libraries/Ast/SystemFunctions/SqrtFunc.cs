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
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        public override Expression Evaluate()
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            var res = Arguments[0].Evaluate();

            if (res is Real)
            {
                if ((res as Real).IsNegative())
                    return ReturnValue(new Complex(new Integer(0), new Irrational(Math.Sqrt((double)(Real)(res as Real).ToNegative()))));
                else
                    return ReturnValue(new Irrational(Math.Sqrt((double)(res as Real)))).Evaluate();
            }

            return new Error(this, "Could not take Sqrt of: " + Arguments[0]);
        }

        public override Expression Reduce()
        {
            if (Exponent.CompareTo(Constant.Two))
            {
                return Arguments[0];
            }
            else
            {
                var simplified = ReduceHelper<SqrtFunc>();

                if (simplified.Arguments[0] is Exp && (simplified.Arguments[0] as Exp).Right.CompareTo(Constant.Two))
                {
                    return (simplified.Arguments[0] as Exp).Left;
                }
                else if (simplified.Arguments[0] is Variable && (simplified.Arguments[0] as Variable).Exponent.CompareTo(Constant.Two))
                {
                    var res = simplified.Arguments[0].Clone() as Variable;
                    res.Exponent = new Integer(1);

                    return res;
                }

                return simplified;
            }
        }

        public override Expression Clone()
        {
            return MakeClone<SqrtFunc>();
        }

        public Expression InvertOn(Expression other)
        {
            return new Exp(other, new Integer(2));
        }
    }
}

