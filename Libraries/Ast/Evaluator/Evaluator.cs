using System;
using System.Collections.Generic;

namespace Ast
{
    public class Evaluator : Scope
    {
        private Parser _parser = new Parser();

        public static Expression Eval(string parseString)
        {
            return new Evaluator(parseString).Evaluate();
        }

        public Evaluator () : this(null) {}
        public Evaluator (string parseString)
        {
            if (parseString != null)
                Parse(parseString);
            SetVar("deg", new Boolean(true));
            SetVar("debug", new Boolean(true));
            SetVar("pi", new Irrational((decimal)Math.PI));

            SetVar("print", new PrintFunc(this));
            SetVar("range", new RangeFunc(this));

            var scope = new Scope(this);
            SetVar("math", scope);
            scope.SetVar("abs", new AbsFunc(this));
            scope.SetVar("solve", new SolveFunc(this));
            scope.SetVar("sqrt", new SqrtFunc(this));

            scope = new Scope(this);
            SetVar("tri", scope);
            scope.SetVar("sin", new SinFunc(this));
            scope.SetVar("cos", new CosFunc(this));
            scope.SetVar("tan", new TanFunc(this));
            scope.SetVar("asin", new AsinFunc(this));
            scope.SetVar("acos", new AcosFunc(this));
            scope.SetVar("atan", new AtanFunc(this));

            scope = new Scope(this);
            SetVar("sys", scope);
            scope.SetVar("eval", new EvalFunc(this));
            scope.SetVar("type", new TypeFunc(this));
            scope.SetVar("expand", new ExpandFunc(this));

            scope = new Scope(this);
            SetVar("draw", scope);
            scope.SetVar("plot", new PlotFunc(this));
            scope.SetVar("paraplot", new ParaPlotFunc(this));
            scope.SetVar("line", new LineFunc(this));


        }

        public void Parse(string parseString)
        {
            Expressions.Clear();
            SideEffects.Clear();
            _parser.Parse(parseString, this);
        }

    }
}