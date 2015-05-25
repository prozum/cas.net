using System;
using System.Collections.Generic;

namespace Ast
{
    public class PlotFunc : SysFunc
    {
        public PlotFunc() : this(null) { }
        public PlotFunc(Scope scope) : base("plot", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Expression,
                    ArgumentType.Variable
                };
        }

        public override Expression Call(List args)
        {
            Expression expr = args[0];
            Variable @var = (Variable)args[1];

            List<Real> xList = new List<Real>();

            foreach (var x in (@var.Value as List).Items)
            {
                if (x is Real)
                    xList.Add(x as Real);
                else
                {
                    CurScope.Errors.Add(new ErrorData(this, "List must only contain real numbers"));
                    return Constant.Null;
                }
            }


            var yList = new List<Real>();

            expr.CurScope = new Scope(expr.CurScope);

            foreach (var x in xList)
            {
                expr.CurScope.SetVar(@var.ToString(), x);

                var res = expr.Evaluate();

                if (CurScope.Error)
                    return Constant.Null;

                if (res is Real)
                    yList.Add(res as Real);
                else
                {
                    CurScope.Errors.Add(new ErrorData(this, "Argument 1 does not return a real number"));
                    return Constant.Null;
                }
            }

            CurScope.SideEffects.Add(new PlotData(xList, yList, null));
            return Constant.Null;
        }
    }
}

