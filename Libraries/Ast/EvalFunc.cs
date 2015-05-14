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

        protected override Expression Evaluate(Expression caller)
        {
            if (!isArgsValid())
                return new ArgumentError(this);

            var arg = args[0].Evaluate();

            if (arg is ErrorExpr)
                return arg;

            if (!(arg is Text))
                return new ErrorExpr("Argument must be Text");

            var res = Evaluator.Eval(arg as Text);

            res.Position.i += args[0].Position.i;
            res.Position.Line += args[0].Position.Line - 1;
            res.Position.Column += args[0].Position.Column;

            return res;
        }
    
        public override Expression Clone()
        {
            return MakeClone<EvalFunc>();
        }
    }
        
}

