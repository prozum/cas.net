using System;
using System.Collections.Generic;

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

        public override bool ContainsVariable(Variable other)
        {
            return false;
        }
    }

    public class Info: Message
    {
        public Info(string message) : base(message)
        {
        }

        public override Expression Clone()
        {
            return new Info(message);
        }
    }

    public class Error: Message
    {
        public Error(string message) : base(message) { }
        public Error(object obj, string message) : base(obj.GetType().Name + "> " +message)
        {
        }

        public override Expression Clone()
        {
            return new Error(message);
        }
    }

    public class ArgError: Error
    {
        public ArgError(string message) : base(message) { }
        public ArgError(SysFunc obj) : base(obj, "Valid args: ")
        {
            message += "[";
            for(int i = 0; i < obj.validArgs.Count; i++)
            {
                message += obj.validArgs[i].ToString();

                if (i < obj.validArgs.Count -1) 
                {
                    message += ',';
                }
            }
            message += "]";
        }

        public override Expression Clone()
        {
            return new ArgError(message);
        }
    }
}

