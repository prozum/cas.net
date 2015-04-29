using System;
using System.Collections.Generic;

namespace Ast
{
    public class Evaluator : Scope
    {
        private Parser _parser = new Parser();

        public Evaluator ()
        {
            SetVar("deg", new Boolean(true));
            SetVar("debug", new Boolean(true));
        }

        public EvalData Evaluation(string inputString)
        {
            Parse(inputString);

            return new ExprData(Evaluate());
        }

        public void Parse(string inputString)
        {
            statements.Clear();
            _parser.Parse(inputString, this);
        }
    }
}