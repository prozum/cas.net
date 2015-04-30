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
            SetVar("PI", new Irrational((decimal)Math.PI));
        }

        public Expression Evaluation(string inputString)
        {
            Parse(inputString);

            return Evaluate();
        }

        public void Parse(string inputString)
        {
            statements.Clear();
            _parser.Parse(inputString, this);
        }

    }
}