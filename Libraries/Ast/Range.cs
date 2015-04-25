using System;
using System.Collections.Generic;

namespace Ast
{
    public class Range : SysFunc
    {
        public Range(List<Expression> args, Scope scope)
            : base("range", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Number,
                    ArgKind.Number,
                    ArgKind.Number
                };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            Decimal start;
            Decimal end;
            Decimal step;

            if (args[0] is Integer)
                start = (args[0] as Integer).value;
            else if (args[0] is Irrational)
                start = (args[0] as Irrational).value;
            else
                return new Error(this, "argument 1 cannot be: " + args[0].GetType().Name);

            if (args[1] is Integer)
                end = (args[1] as Integer).value;
            else if (args[1] is Irrational)
                end = (args[1] as Irrational).value;
            else
                return new Error(this, "argument 2 cannot be: " + args[1].GetType().Name);

            if (args[2] is Integer)
                step = (args[2] as Integer).value;
            else if (args[2] is Irrational)
                step = (args[2] as Irrational).value;
            else
                return new Error(this, "argument 3 cannot be: " + args[2].GetType().Name);

            var list = new Ast.List ();
            for (Decimal i = start; i < end; i += step)
            {
                list.elements.Add(new Irrational(i));
            }

            return list;
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

