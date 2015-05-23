using System;
using System.Collections.Generic;

namespace Ast
{
    public class PrintFunc : SysFunc
    {
        public PrintFunc() : this(null, null) { }
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

            var res = Arguments[0].Evaluate().ToString();

            CurScope.SideEffects.Add(new PrintData(res));

            return new Text(res);
        }

        public override Expression Clone()
        {
            return MakeClone<PrintFunc>();
        }
    }
}

