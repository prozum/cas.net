using System;
using System.Collections.Generic;

namespace Ast
{
    public class EvalFunc : SysFunc
    {
        public EvalFunc(List<Expression> args, Scope scope)
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

            var arg = args[0].Evaluate();

            if (arg is Error)
                return arg;

            if (!(arg is Text))
                return new Error("Argument must be Text");

            var res = Evaluator.Eval(arg as Text);

            res.pos.i += args[0].pos.i;
            res.pos.Line += args[0].pos.Line - 1;
            res.pos.Column += args[0].pos.Column;

            return res;
        }
    }
}

