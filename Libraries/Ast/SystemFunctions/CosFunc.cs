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
            var res = args[0].Evaluate();

            var deg = CurScope.GetBool("deg");

            if (res is Real)
            {
                return new Irrational(Math.Cos((double) ((deg ? Constant.DegToRad.@decimal  : 1) * (res as Real)) )).Evaluate();
            }

            CurScope.Errors.Add(new ErrorData(this, "Could not take Cos of: " + args[0]));
            return Constant.Null;
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
            return SysFunc.MakeFunction<AcosFunc>(arg, other.CurScope);
        }
    }
}

