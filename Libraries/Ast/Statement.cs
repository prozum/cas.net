using System;

namespace Ast
{
    public abstract class Statement
    {
        public Statement()
        {
        }

        public abstract EvalData Step();

        public abstract Expression Evaluate();
    }
}

