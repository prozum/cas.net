using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
    public abstract class Expression
    {
        public Evaluator evaluator;
        public Operator parent;
        public UserDefinedFunction functionCall;
        public abstract Expression Evaluate();

        public virtual void SetFunctionCall(UserDefinedFunction functionCall)
        {
            this.functionCall = functionCall;
        }

        public virtual Expression Expand()
        {
            return this;
        }

        public virtual Expression Simplify()
        {
            return this;
        }

        public virtual bool CompareTo(Expression other)
        {
            if (this.GetType() == other.GetType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract bool ContainsNotNumber(NotNumber other);

        //public abstract string ToString ();
    }
}