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

        public override Expression Reduce(List args, Scope scope)
        {
            var reduced = args.Reduce() as List;

            if (reduced[0] is Exp && (reduced[0] as Exp).Right.CompareTo(Constant.Two))
                return (reduced[0] as Exp).Left;

            return this;
        }


        public Expression InvertOn(Expression other)
        {
            return new Exp(other, new Integer(2));
        }
    }
}

