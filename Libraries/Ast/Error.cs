using System;

namespace Ast
{
    public class Error : Expression
    {
        public string message;

        public Error (string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return message;
        }

        public override Expression Evaluate()
        {
            return this;
        }

        public override bool CompareTo(Expression other)
        {
            return false;
        }
    }
}

