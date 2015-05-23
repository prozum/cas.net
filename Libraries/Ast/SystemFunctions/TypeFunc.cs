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

        public override bool CompareTo(Expression other)
        {
            var eval = Evaluate();

            if (eval is Text)
            {
                if (other is Text)
                {
                    return ((eval as Text).@string.CompareTo((other as Text).@string) == 0) ? true : false;
                }
                if (other is TypeFunc)
                {
                    var text = (other as TypeFunc).Evaluate();

                    if (text is Text)
                    {
                        return ((eval as Text).@string.CompareTo((text as Text).@string) == 0) ? true : false;
                    }
                }
            }

            return base.CompareTo(other);
        }
    }
}

