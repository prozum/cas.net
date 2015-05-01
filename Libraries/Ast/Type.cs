using System;
using System.Collections.Generic;

namespace Ast
{
    public class Type : SysFunc
    {
        public Type(List<Expression> args, Scope scope)
            : base("type", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }
            
        public override EvalData Step()
        {
            if (!isArgsValid())
                return new ErrorData(new ArgError(this));

            if (!stepped)
            {
                stepped = true;
                return new PrintData(args[0].GetType().Name);
            }

            stepped = false;
            return new DoneData();
        }
    }
}

