using System;
using System.Collections.Generic;

namespace Ast
{
    public class TanFunc : SysFunc, IInvertable
    {
        public TanFunc() : this(null) { }
        public TanFunc(Scope scope) : base("tan", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Real
                };
        }

        public override Expression Call(List args)
        {
            var res = args[0].Evaluate();

            var deg = GetBool("deg");

            if (res is Real)
            {
                if (res.CompareTo(Constant.Deg26d57))
                    return Constant.Half;

                return new Irrational(Math.Tan((double)((deg ? Constant.DegToRad.@decimal : 1) * (res as Real)))).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + args[0]);
        }

        public override Expression Reduce(List args, Scope scope)
        {
            var reduced = args.Reduce() as List;

            if (reduced[0] is Call && (reduced[0] as Call).Child.Value is AtanFunc)
                return (reduced[0] as Call).Arguments[0];

            return this;
        }

        // tan[x] -> atan[other]
        public Expression InvertOn(Expression other)
        {
            var arg = new List();
            arg.Items.Add(other);
            return SysFunc.MakeFunction(arg, CurScope, "atan");
        }
    }
}

