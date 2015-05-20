using System;
using System.Collections.Generic;

namespace Ast
{
    public class ParaPlotFunc : SysFunc
    {
        public Expression expr1;
        public Expression expr2;
        public Variable @var;

        public ParaPlotFunc() : this(null, null) { }
        public ParaPlotFunc(List<Expression> args, Scope scope)
            : base("paraplot", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression,
                    ArgKind.Expression,
                    ArgKind.Variable
                };

            if (IsArgumentsValid())
            {
                expr1 = args[0];
                expr2 = args[1];
                @var = (Variable)args[2];
            }
        }

        public override Expression Evaluate()
        {
            List<Real> xList = new List<Real>();
            List<Real> yList = new List<Real>();
            List<Real> zList = new List<Real>();

            foreach (var z in (@var.Value as List).items)
            {
                if (z is Real)
                    zList.Add(z as Real);
                else
                    return new Error(this, "List must only contain real numbers");
            }


            expr1.Scope = new Scope(expr1.Scope);
            expr2.Scope = new Scope(expr2.Scope);

            foreach (var z in zList)
            {
                expr1.Scope.SetVar(@var.ToString(), z);
                expr2.Scope.SetVar(@var.ToString(), z);

                var res1 = expr1.Evaluate();
                var res2 = expr2.Evaluate();

                if (res1 is Error)
                    return res1;
                if (res1 is Real)
                    xList.Add(res1 as Real);
                else
                    return new Error(this, "Argument 1 returned a none real number:" + res1);

                if (res2 is Error)
                    return res2;
                if (res2 is Real)
                    yList.Add(res2 as Real);
                else
                    return new Error(this, "Argument 2 returned a none real number:" + res2);
            }

            Scope.SideEffects.Add(new PlotData(xList, yList, zList));
            return new Null();
        }

        public override Expression Clone()
        {
            return MakeClone<ParaPlotFunc>();
        }
    }
}

