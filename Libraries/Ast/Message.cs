using System;
using System.Collections.Generic;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
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
    }

    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    public class Error: Message
    {
        public Error(string msg) : base(msg) { }
        public Error(Expression expr, string msg)
        {
            pos = expr.pos;

            if (expr is Variable)
                this.msg = (expr as Variable).identifier + ": " + msg;
            else
                this.msg = expr.GetType().Name + ": " + msg;
        }

        public override Expression Clone()
        {
            return new Error(this, msg);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ArgError: Error
    {
        public ArgError(string msg) : base(msg) { }
        public ArgError(SysFunc func) : base(func, "Valid args: ")
        {
            msg += "[";
            for(int i = 0; i < func.validArgs.Count; i++)
            {
                msg += func.validArgs[i].ToString();

                if (i < func.validArgs.Count -1) 
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

