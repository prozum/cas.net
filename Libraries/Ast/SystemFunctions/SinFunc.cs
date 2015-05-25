using System;
using System.Collections.Generic;

namespace Ast
{
    public class SinFunc : SysFunc, IInvertable
    {
        public SinFunc() : this(null) { }
        public SinFunc(Scope scope) : base("sin", scope)
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
                return new Irrational(Math.Sin((double) ((deg ? Constant.DegToRad.@decimal  : 1) * (res as Real)) )).Evaluate();
            }

            CurScope.Errors.Add(new ErrorData(this, "Could not take Sin of: " + args[0]));
            return Constant.Null;
        }

        public override Expression Reduce(List args, Scope scope)
        {
            var reduced = args.Reduce() as List;

            if (reduced[0] is Call && (reduced[0] as Call).Child.Value is AsinFunc)
                return (reduced[0] as Call).Arguments[0];

            return this;
        }

        //sin[x] -> asin[other]
        public Expression InvertOn(Expression other)
        {
            var arg = new List();
            arg.Items.Add(other);
            return SysFunc.MakeFunction<AsinFunc>(arg, other.CurScope);
        }
    }
}

