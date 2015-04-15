using System;

namespace Ast
{
    public abstract class Message : Expression
    {
        public string message;

        public Message (string message)
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

        public override bool ContainsNotNumber(NotNumber other)
        {
            return true;
        }
    }

    public class Info: Message
    {
        public Info(string message) : base(message)
        {
        }
    }

    public class Error: Message
    {
        public Error(string message) : base(message)
        {
        }
    }
}

