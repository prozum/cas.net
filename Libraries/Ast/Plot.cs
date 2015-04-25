﻿using System;
using System.Collections.Generic;

namespace Ast
{
    public class Plot : SysFunc
    {
        public Expression exp;
        public Symbol sym;

        public Plot(List<Expression> args, Scope scope)
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

        public override Expression Evaluate()
        {
            return new Error(this, "Cannot evaluate plot");
        }
    }
}
