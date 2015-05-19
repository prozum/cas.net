using System;
using System.Collections.Generic;

namespace Ast
{
    public class LineFunc : SysFunc
    {
        public decimal x1;
        public decimal y1;

        public decimal x2;
        public decimal y2;

        public LineFunc() : this(null, null) { }
        public LineFunc(List<Expression> args, Scope scope)
            : base("line", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Real,
                    ArgKind.Real,
                    ArgKind.Real,
                    ArgKind.Real,
                };

            if (IsArgumentsValid())
            {
                x1 = args[0] as Real;
                y1 = args[1] as Real;
                x2 = args[2] as Real;
                y2 = args[3] as Real;
            }
        }

        public override Expression Evaluate()
        {

            Scope.SideEffects.Add(new LineData(x1,y1,x2,y2));
            return new Null();
        }

        public override Expression Clone()
        {
            return MakeClone<LineFunc>();
        }
    }
}

