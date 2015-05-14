using System;
using System.Collections.Generic;

namespace Ast
{
    public class PrintFunc : FuncStmt
    {
        public PrintFunc(List<Expression> args, Scope scope)
            : base("print", args, scope)
        {
            ValidArguments = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override EvalData Evaluate()
        {
            if (!IsArgumentsValid())
                return new ErrorData(new ArgumentError(this));

            return new PrintData(Arguments[0].Evaluate().ToString());
        }

    }
}

