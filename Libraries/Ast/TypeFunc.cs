using System;
using System.Collections.Generic;

namespace Ast
{
    public class TypeFunc : SysFunc
    {
        public TypeFunc(List<Expression> args, Scope scope)
            : base("type", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is ErrorExpr)
                return res;
            else
                return new Text(args[0].Evaluate().GetType().Name);
        }
    }
}

