﻿using System;
using System.Collections.Generic;

namespace Ast
{
    public class PrintFunc : SysFunc
    {
        public PrintFunc(List<Expression> args, Scope scope)
            : base("print", args, scope)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override EvalData Step()
        {
            if (!isArgsValid())
                return new ErrorData(new ArgError(this));
                

            if (!stepped)
            {
                stepped = true;
                return new PrintData(args[0].Evaluate().ToString());
            }

            stepped = false;
            return new DoneData();
        }

    }
}
