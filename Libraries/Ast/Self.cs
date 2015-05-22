﻿using System;

namespace Ast
{
    public class Self : Expression
    {
        public Self()
        {
        }

        public override string ToString()
        {
            return "self";
        }

        public override Expression Evaluate()
        {
            return Scope;
        }

        internal override Expression Evaluate(Expression caller)
        {
            return this;
        }
    }
}
