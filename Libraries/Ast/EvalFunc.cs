using System;
using System.Collections.Generic;

namespace Ast
{
    public class EvalFunc : SysFunc
    {
        public EvalFunc() : this(null, null) { }
        public EvalFunc(List<Expression> args, Scope scope)
            : base("eval", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        internal override Expression Evaluate(Expression caller)
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            var arg = Arguments[0].Evaluate();

            if (arg is Error)
                return arg;

            if (!(arg is Text))
                return new Error("Argument must be Text");

            var res = Evaluator.Eval(arg as Text);

            res.Position.i += Arguments[0].Position.i;
            res.Position.Line += Arguments[0].Position.Line - 1;
            res.Position.Column += Arguments[0].Position.Column;

            return res;
        }
    
        public override Expression Clone()
        {
            return MakeClone<EvalFunc>();
        }
    }
        
}

