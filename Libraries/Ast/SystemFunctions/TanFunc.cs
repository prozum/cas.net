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

            var deg = CurScope.GetBool("deg");

            if (res is Real)
            {
                return new Irrational(Math.Tan((double) ((deg ? Constant.DegToRad.@decimal  : 1) * (res as Real)) )).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + args[0]);
        }

        public Expression InvertOn(Expression other)
        {
            throw new NotImplementedException();
//            List<Expression> newArgs = new List<Expression>();
//            newArgs.Add(other);
//            return new AtanFunc(newArgs, CurScope);
        }
    }
}

