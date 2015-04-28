using System;
using System.Collections.Generic;

namespace Ast
{
    public class Evaluator
    {
        private Parser _parser = new Parser();
        public Scope scope = new Scope();

        public Evaluator ()
        {
            scope.SetVar("deg", new Boolean(true));
            scope.SetVar("debug", new Boolean(true));
        }

        public EvalData Evaluation(string inputString)
        {
            Parse(inputString);

            return new ExprData(scope.Evaluate());
        }

        public void Parse(string inputString)
        {
            scope.statements.Clear();
            _parser.Parse(inputString, scope);
        }

        public EvalData Step()
        {
            return scope.Step();
        }
    }
}

