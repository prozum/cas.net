using System;
using System.Collections.Generic;

namespace Ast
{
    public class Error : Expression
    {
        public string ErrorMessage;

        public Error(string msg) 
        {
            this.ErrorMessage = msg;
        }

        public Error(Expression expr, string msg) : this(expr, expr.Position, msg) { }
        public Error(Statement stmt, string msg) : this(stmt, stmt.Position, msg) { }

        public Error (Object obj, Pos Position, string msg)
        {
            if (obj is Variable)
                this.ErrorMessage = (obj as Variable).Identifier + ": " + msg;
            else
                this.ErrorMessage = obj.GetType().Name + ": " + msg;
        }

        public override string ToString()
        {
            return ErrorMessage;
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

        public override Expression Clone()
        {
            return new Error(ErrorMessage);
        }
    }

    public class ArgumentError: Error
    {
        public ArgumentError(SysFunc func) : this(func, func.Position, func.ValidArguments) { }

        public ArgumentError(object obj, Pos position, List<ArgKind> validArgs) : base(obj, position, "Valid args: ")
        {
            ErrorMessage += "[";
            for(int i = 0; i < validArgs.Count; i++)
            {
                ErrorMessage += validArgs[i].ToString();

                if (i < validArgs.Count -1) 
                {
                    ErrorMessage += ',';
                }
            }
            ErrorMessage += "]";
        }
    }
}

