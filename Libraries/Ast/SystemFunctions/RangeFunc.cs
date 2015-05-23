using System;
using System.Collections.Generic;

namespace Ast
{
    public class RangeFunc : SysFunc
    {
        public RangeFunc() : this(null, null) { }
        public RangeFunc(List<Expression> args, Scope scope)
            : base("range", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Real,
                    ArgKind.Real,
                    ArgKind.Real
                };
        }

        internal override Expression Evaluate(Expression caller)
        {
            if (!IsArgumentsValid())
                return new ArgumentError(this);

            Decimal start;
            Decimal end;
            Decimal step;

            start = Arguments[0].Evaluate() as Real;

            end = Arguments[1].Evaluate() as Real;

            step = Arguments[2].Evaluate() as Real;

            var list = new Ast.List ();
            for (Decimal i = start; i <= end; i += step)
            {
                list.items.Add(new Irrational(i));
            }

            return list;
        }

        public override Expression Clone()
        {
            return MakeClone<RangeFunc>();
        }
    }
}

