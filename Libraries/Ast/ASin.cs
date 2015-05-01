using System;
using System.Collections.Generic;

namespace Ast
{
    public class ASin : SysFunc, IInvertable
    {
        public ASin() : this(null, null) { }
        public ASin(List<Expression> args, Scope scope)
            : base("asin", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var deg = scope.GetBool("deg");

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Asin((double) ((deg ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take ASin of: " + args[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<ASin>();
        }

        public override Expression Clone()
        {
            return MakeClone<ASin>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new Sin(newArgs, scope);
        }
    }
}

