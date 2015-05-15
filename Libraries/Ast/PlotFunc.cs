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
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression,
                    ArgKind.Symbol
                };

            if (IsArgumentsValid())
            {
                exp = args[0];
                sym = (Symbol)args[1];
            }
        }

        public override Expression Evaluate()
        {
            return new Null();
        }
    }
}

