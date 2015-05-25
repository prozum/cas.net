using System;
using System.Collections.Generic;

namespace Ast
{
    public class ParaPlotFunc : SysFunc
    {
        public ParaPlotFunc() : this(null) { }
        public ParaPlotFunc(Scope scope)
            : base("paraplot", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Expression,
                    ArgumentType.Expression,
                    ArgumentType.Variable
                };
        }

        public override Expression Call(List args)
        {
            Expression expr1 = args[0];
            Expression expr2 = args[1];
            Variable @var = (Variable)args[2];

            List<Real> xList = new List<Real>();
            List<Real> yList = new List<Real>();
            List<Real> zList = new List<Real>();

            foreach (var z in (@var.Value.Value as List).Items)
            {
                if (z is Real)
                    zList.Add(z as Real);
                else
                {
                    CurScope.Errors.Add(new ErrorData(this, "List must only contain real numbers"));
                    return Constant.Null;
                }
            }


            expr1.CurScope = new Scope(CurScope);
            expr2.CurScope = new Scope(CurScope);

            foreach (var z in zList)
            {
                expr1.CurScope.SetVar(@var.ToString(), z);
                expr2.CurScope.SetVar(@var.ToString(), z);

                var res1 = expr1.Evaluate();
                if (CurScope.Error)
                    return Constant.Null;
                var res2 = expr2.Evaluate();
                if (CurScope.Error)
                    return Constant.Null;
                    
                if (res1 is Real)
                    xList.Add(res1 as Real);
                else
                {
                    CurScope.Errors.Add(new ErrorData(this, "Argument 1 returned a none real number:" + res1));
                    return Constant.Null;
                }
                    
                if (res2 is Real)
                    yList.Add(res2 as Real);
                else
                {
                    CurScope.Errors.Add(new ErrorData(this, "Argument 2 returned a none real number:" + res2));
                    return Constant.Null;
                }
            }

            CurScope.SideEffects.Add(new PlotData(xList, yList, zList));
            return Constant.Null;
        }
    }
}

