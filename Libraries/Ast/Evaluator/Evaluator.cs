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
            SetVar("sin", new SinFunc(this));
        }

        public void Parse(string parseString)
        {
            Expressions.Clear();
            SideEffects.Clear();
            _parser.Parse(parseString, this);
        }

    }
}