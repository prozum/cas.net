using System;
using System.Collections.Generic;

namespace Ast
{
    public class SqrtFunc : SysFunc, IInvertable
    {
        public SqrtFunc() : this(null) { }
        public SqrtFunc(Scope scope) : base("sqrt", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Expression
                };
        }

        public override Expression Call(List args)
        {
            var res = args[0].Evaluate();

            if (res is Real)
            {
                if ((res as Real).IsNegative())
                    return new Complex(new Integer(0), new Irrational(Math.Sqrt((double)(Real)(res as Real).ToNegative())));
                else
                    return new Irrational(Math.Sqrt((double)(res as Real))).Evaluate();
            }

            return new Error(this, "Could not take Sqrt of: " + args[0]);
        }

        public override Expression Reduce()
        {
            throw new NotImplementedException();
//            var simplified = ReduceHelper<SqrtFunc>();
//
//            // sqrt[x^2] -> x
//            if (simplified.Arguments[0] is Exp && (simplified.Arguments[0] as Exp).Right.CompareTo(Constant.Two))
//            {
//                return (simplified.Arguments[0] as Exp).Left;
//            }
//            // sqrt[x^2] -> x
//            else if (simplified.Arguments[0] is Variable && (simplified.Arguments[0] as Variable).Exponent.CompareTo(Constant.Two))
//            {
//                var res = simplified.Arguments[0].Clone() as Variable;
//                res.Exponent = new Integer(1);
//
//                return res;
//            }

//            return simplified;

        }


        public Expression InvertOn(Expression other)
        {
            return new Exp(other, new Integer(2));
        }
    }
}

