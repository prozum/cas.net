using System;
using System.Collections.Generic;

namespace Ast
{
    public class Evaluator
    {
        public Scope scope;

        public Evaluator ()
        {
            scope = new Scope();
            scope.SetVar("deg", new Boolean(true));
        }

        public EvalData Evaluation(string inputString)
        {
            Parse(inputString);

            return new ExpData(scope.Evaluate());
        }

        public void Parse(string inputString)
        {
            scope.statements.Clear();
            Parser.Parse(inputString, scope);
        }

        public EvalData Step()
        {
            return scope.Step();
        }
    }
}

