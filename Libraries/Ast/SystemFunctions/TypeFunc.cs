using System;
using System.Collections.Generic;

namespace Ast
{
    public class TypeFunc : SysFunc
    {
        public TypeFunc() : this(null) { }
        public TypeFunc(Scope scope) : base("type", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Expression
                };
        }

        public override Expression Call(List args)
        {
            var res = args[0].Evaluate();

            if (res is Error)
                return res;
            else
                return new Text(res.GetType().Name);
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

