using System;
using System.Collections.Generic;

namespace Ast
{
    public class EvalFunc : SysFunc
    {
        public EvalFunc() : this(null) { }
        public EvalFunc(Scope scope) : base("eval", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Text
                };
        }

        public override Expression Call(List args)
        {
            var arg = args[0].Evaluate();

            if (CurScope.Error)
                return Constant.Null;

            var res = Evaluator.Eval(arg as Text);

            res.Position.i += args[0].Position.i;
            res.Position.Line += args[0].Position.Line - 1;
            res.Position.Column += args[0].Position.Column;

            return res;
        }
    }
        
}

