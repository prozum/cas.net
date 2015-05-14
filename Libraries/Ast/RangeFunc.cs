using System;
using System.Collections.Generic;

namespace Ast
{
    public class RangeFunc : SysFunc
    {
        public RangeFunc(List<Expression> args, Scope scope)
            : base("range", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Number,
                    ArgKind.Number,
                    ArgKind.Number
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            if (!isArgsValid())
                return new ArgumentError(this);

            Decimal start;
            Decimal end;
            Decimal step;

            if (args[0] is Integer)
                start = (args[0] as Integer).@int;
            else if (args[0] is Irrational)
                start = (args[0] as Irrational).@decimal;
            else
                return new Error(this, "argument 1 cannot be: " + args[0].GetType().Name);

            if (args[1] is Integer)
                end = (args[1] as Integer).@int;
            else if (args[1] is Irrational)
                end = (args[1] as Irrational).@decimal;
            else
                return new Error(this, "argument 2 cannot be: " + args[1].GetType().Name);

            if (args[2] is Integer)
                step = (args[2] as Integer).@int;
            else if (args[2] is Irrational)
                step = (args[2] as Irrational).@decimal;
            else
                return new Error(this, "argument 3 cannot be: " + args[2].GetType().Name);

            var list = new Ast.List ();
            for (Decimal i = start; i < end; i += step)
            {
                list.items.Add(new Irrational(i));
            }

            return list;
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

