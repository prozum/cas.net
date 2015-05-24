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

            return new Error(this, "Could not take Sin of: " + args[0]);
        }

        public Expression InvertOn(Expression other)
        {
            throw new NotImplementedException();
//            List<Expression> newArgs = new List<Expression>();
//            newArgs.Add(other);
//            return new AsinFunc(newArgs, CurScope);
        }
    }
}

