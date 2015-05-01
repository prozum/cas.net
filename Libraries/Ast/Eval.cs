using System;
using System.Collections.Generic;

namespace Ast
{
    public class Eval : SysFunc
    {
        public Eval(List<Expression> args, Scope scope)
            : base("eval", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Error)
                return res;

            if (!(res is Text))
                return new Error("Argument must be Text");

            return  new Evaluator((res as Text)).Evaluate();
        }
    }
}

