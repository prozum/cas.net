using System;
using System.Collections.Generic;

namespace Ast
{
    public class CosFunc : SysFunc, IInvertable
    {
        public CosFunc() : this(null) { }
        public CosFunc(Scope scope)
            : base("cos", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Real
                };
        }

        public override Expression Call(List args)
        {
            if (!IsArgumentsValid(args))
                return new ArgumentError(this);

            var res = args[0].Evaluate();

            var deg = GetBool("deg");

            if (res is Real)
            {
                double value = res as Real;

                if (value == (90 * (deg ? 1.0 : (double)Constant.DegToRad.@decimal)))
                    return Constant.Zero;

                return new Irrational(Math.Cos((double)(deg ? Constant.DegToRad.@decimal : 1) * value)).Evaluate();
            }

            return new Error(this, "Could not take Cos of: " + args[0]);
        }

        public override Expression Reduce(List args, Scope scope)
        {
            var reduced = args.Reduce() as List;

            if (reduced[0] is Call && (reduced[0] as Call).Child.Value is AcosFunc)
                return (reduced[0] as Call).Arguments[0];

            return this;
        }

        //cos[x] -> acos[other]
        public Expression InvertOn(Expression other)
        {
            var arg = new List();
            arg.Items.Add(other);
            return SysFunc.MakeFunction(arg, CurScope, "acos");
        }
    }
}

