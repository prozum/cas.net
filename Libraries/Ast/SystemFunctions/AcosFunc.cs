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
            if (!IsArgumentsValid(args))
                return new ArgumentError(this);

            var res = args[0].Evaluate();

            var deg = CurScope.GetBool("deg");

            if (res is Real)
            {
                double value = res as Real;

                if (value >= -1 && value <= 1)
                    return new Irrational((decimal)Math.Acos(value) * (deg ? Constant.RadToDeg.@decimal  : 1)).Evaluate();
            }

            return new Error(this, "Could not take ACos of: " + args[0]);
        }

        //acos[x] -> cos[other]
        public Expression InvertOn(Expression other)
        {
            throw new NotImplementedException();
            //List<Expression> newArgs = new List<Expression>();
            //newArgs.Add(other);
            return new CosFunc(CurScope);
        }
    }
}

