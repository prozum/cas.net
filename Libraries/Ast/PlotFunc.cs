using System;
using System.Collections.Generic;

namespace Ast
{
    public class PlotFunc : SysFunc
    {
        public Expression expr;
        public Variable @var;

        public PlotFunc() : this(null, null) { }
        public PlotFunc(List<Expression> args, Scope scope)
            : base("plot", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Expression,
                    ArgKind.Variable
                };

            if (IsArgumentsValid())
            {
                expr = args[0];
                @var = (Variable)args[1];
            }
        }

        public override Expression Evaluate()
        {
            List<Real> xList = new List<Real>();

            foreach (var x in (@var.Value as List).items)
            {
                if (x is Real)
                    xList.Add(x as Real);
                else
                    return new Error(this, "List must only contain real numbers");
            }


            var yList = new List<Real>();

            expr.Scope = new Scope(expr.Scope);

            foreach (var x in xList)
            {
                expr.Scope.SetVar(@var.ToString(), x);

                var res = expr.Evaluate();

                if (res is Error)
                    return res;
                if (res is Real)
                    yList.Add(res as Real);
                else
                    return new Error(this, "Argument 1 does not return a real number");
            }

            Scope.SideEffects.Add(new PlotData(xList, yList));
            return new Null();
        }

        public override Expression Clone()
        {
            return MakeClone<PlotFunc>();
        }
    }
}

