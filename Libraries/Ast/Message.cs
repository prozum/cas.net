using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class Message : Expression
    {
        public string msg;

        public Message () : this("Empty message") {}
        public Message (string msg)
        {
            this.msg = msg;
        }

        public override string ToString()
        {
            return msg;
        }


        public override Expression Evaluate()
        {
            return this;
        }
        protected override Expression Evaluate(Expression caller)
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
            return new Info(msg);
        }
    }

    public class Error: Message
    {
        public Error(string msg) : base(msg) { }
        public Error(object obj, string msg)
        {
            var str = "";

            if (obj is Expression)
            {
                str += "[" +(obj as Expression).pos.Column;
                str += ";" +(obj as Expression).pos.Line + "]";
                str += obj.GetType().Name + ": ";
                str += msg;
            }

            this.msg = str;
        }

        public override Expression Clone()
        {
            return new Error(msg);
        }
    }

    public class ArgError: Error
    {
        public ArgError(string msg) : base(msg) { }
        public ArgError(SysFunc obj) : base(obj, "Valid args: ")
        {
            msg += "[";
            for(int i = 0; i < obj.validArgs.Count; i++)
            {
                msg += obj.validArgs[i].ToString();

                if (i < obj.validArgs.Count -1) 
                {
                    msg += ',';
                }
            }
            msg += "]";
        }

        public override Expression Clone()
        {
            return new ArgError(msg);
        }
    }
}

