using System;
using System.Collections.Generic;

namespace Ast
{
    public class AsinFunc : SysFunc, IInvertable
    {
        public AsinFunc() : this(null) { }
        public AsinFunc(Scope scope)
            : base("asin", scope)
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
                    return new Irrational((decimal)Math.Asin(value) * (deg ? Constant.RadToDeg.@decimal  : 1)).Evaluate();
            }

            return new Error(this, "Could not take ASin of: " + args[0]);
        }

        //asin[x] -> sin[other]
        public Expression InvertOn(Expression other)
        {
            throw new NotImplementedException();
//            List<Expression> newArgs = new List<Expression>();
//            newArgs.Add(other);
//            return new SinFunc(newArgs, CurScope);
        }
    }
}

