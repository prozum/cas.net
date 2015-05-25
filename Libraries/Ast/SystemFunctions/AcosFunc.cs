using System;
using System.Collections.Generic;

namespace Ast
{
    public class AcosFunc : SysFunc, IInvertable
    {
        public AcosFunc() : this(null) { }
        public AcosFunc(Scope scope)
            : base("acos", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Real
                };
        }

        public override Expression Call(List args)
        {
            var res = args[0].Evaluate();

            var deg = CurScope.GetBool("deg");

            if (res is Real)
            {
                double value = res as Real;

                if (value >= -1 && value <= 1)
                    return new Irrational((decimal)Math.Acos(value) * (deg ? Constant.RadToDeg.@decimal  : 1)).Evaluate();
            }

            CurScope.Errors.Add(new ErrorData(this, "Could not take ACos of: " + args[0]));
            return Constant.Null;
        }

        public override Expression Reduce(List args, Scope scope)
        {
            var reduced = args.Reduce() as List;

            if (reduced[0] is Call && (reduced[0] as Call).Child.Value is CosFunc)
                return (reduced[0] as Call).Arguments[0];

            return this;
        }

        //acos[x] -> cos[other]
        public Expression InvertOn(Expression other)
        {
            var arg = new List();
            arg.Items.Add(other);
            return SysFunc.MakeFunction<CosFunc>(arg, other.CurScope);
        }
    }
}

