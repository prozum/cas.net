using System;

namespace Ast
{
    public abstract class State
    {
        public State()
        {
        }

        public abstract EvalData Step();

        public abstract Expression Evaluate();
    }
}

