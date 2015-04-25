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
            scope.statements.Clear();
            Parser.Parse(inputString, scope);
            var exp = scope.Evaluate();

            if (exp is Error)
            {
                return new MsgData(MsgType.Error, exp.ToString());
            }

            if (exp is Info)
            {
                return new MsgData(MsgType.Info, exp.ToString());
            }

            return new MsgData(MsgType.Print, exp.ToString());
        }
    }
}

