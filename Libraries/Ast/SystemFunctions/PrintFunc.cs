using System;
using System.Collections.Generic;

namespace Ast
{
    public class PrintFunc : SysFunc
    {
        public PrintFunc() : this(null) { }
        public PrintFunc(Scope scope) : base("print", scope)
        {
            ValidArguments = new List<ArgumentType>()
            {
                ArgumentType.Expression
            };
        }

        public override Expression Call(List args)
        {
            var res = args[0].Evaluate().ToString();

            CurScope.SideEffects.Add(new PrintData(res));

            return new Text(res);
        }
    }
}

