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

            var deg = CurScope.GetBool("deg");

            if (res is Real)
            {
                return new Irrational(Math.Cos((double) ((deg ? Constant.DegToRad.@decimal  : 1) * (res as Real)) )).Evaluate();
            }

            return new Error(this, "Could not take Cos of: " + args[0]);
        }

        public Expression InvertOn(Expression other)
        {
            throw new NotImplementedException();
//            List<Expression> newArgs = new List<Expression>();
//            newArgs.Add(other);
//            return new AcosFunc(newArgs, CurScope);
        }
    }
}

