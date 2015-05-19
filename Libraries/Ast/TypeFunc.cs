using System;
using System.Collections.Generic;

namespace Ast
{
    public class TypeFunc : SysFunc
    {
        public TypeFunc() : this(null, null) { }
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
            var res = Arguments[0].Evaluate();

            if (res is Error)
                return res;
            else
                return new Text(res.GetType().Name);
        }

        public override Expression Clone()
        {
            return MakeClone<TypeFunc>();
        }
    }
}

