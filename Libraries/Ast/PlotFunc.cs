using System;
using System.Collections.Generic;

namespace Ast
{
    public class PlotFunc : SysFunc
    {
        public Expression exp;
        public Symbol sym;

        public PlotFunc(List<Expression> args, Scope scope)
            : base("plot", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression,
                    ArgKind.Symbol
                };

            if (isArgsValid())
            {
                exp = args[0];
                sym = (Symbol)args[1];
            }
        }

        protected override Expression Evaluate(Expression caller)
        {
            return new Error(this, "Cannot evaluate plot");
        }
    }
}

