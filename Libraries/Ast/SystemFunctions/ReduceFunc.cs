using System.Collections.Generic;

namespace Ast
{
    public class ReduceFunc : SysFunc
    {
        public ReduceFunc() : this(null) { }
        public ReduceFunc(Scope scope) : base("reduce", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Expression
                };
        }

        public override Expression Call(List args)
        {
            return args[0].ReduceCurrectOp();
        }
    }
}

