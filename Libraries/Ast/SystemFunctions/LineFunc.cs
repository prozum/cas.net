using System;
using System.Collections.Generic;

namespace Ast
{
    public class LineFunc : SysFunc
    {
        public LineFunc() : this(null) { }
        public LineFunc(Scope scope) : base("line", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Real,
                    ArgumentType.Real,
                    ArgumentType.Real,
                    ArgumentType.Real,
                };
        }

        public override Expression Call(List args)
        {
            var x1 = args[0] as Real;
            var y1 = args[1] as Real;
            var x2 = args[2] as Real;
            var y2 = args[3] as Real;
            CurScope.SideEffects.Add(new LineData(x1,y1,x2,y2));
            return Constant.Null;
        }
    }
}

