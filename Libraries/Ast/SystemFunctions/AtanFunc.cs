using System;
using System.Collections.Generic;

namespace Ast
{
    public class AtanFunc : SysFunc, IInvertable
    {
        public AtanFunc() : this(null) { }
        public AtanFunc(Scope scope) : base("atan", scope)
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
                return new Irrational((decimal)Math.Atan((double)(res as Real)) * (deg ? Constant.RadToDeg.@decimal  : 1) ).Evaluate();
            }

            return new Error(this, "Could not take ATan of: " + args[0]);
        }

        public Expression InvertOn(Expression other)
        {
            throw new NotImplementedException();
            //List<Expression> newArgs = new List<Expression>();
            //newArgs.Add(other);
            return new TanFunc(CurScope);
        }
    }
}

