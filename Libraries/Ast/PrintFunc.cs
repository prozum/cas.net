using System;
using System.Collections.Generic;

namespace Ast
{
    public class PrintFunc : SysFunc
    {
        public PrintFunc(List<Expression> args, Scope scope)
            : base("print", args, scope)
        {
            ValidArguments = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            Scope.SideEffects.Add(new PrintData(Arguments[0].Evaluate().ToString()));

            return new Null();
        }

    }
}

