using System;
using System.Collections.Generic;

namespace Ast
{
    public class ErrorExpr : Expression
    {
        public string ErrorMessage;

        public ErrorExpr(string msg) 
        {
            this.ErrorMessage = msg;
        }

        public ErrorExpr(Expression expr, string msg) : this(expr, expr.Position, msg) { }
        public ErrorExpr(Statement stmt, string msg) : this(stmt, stmt.Position, msg) { }

        public ErrorExpr (Object obj, Pos Position, string msg)
        {
            if (obj is Variable)
                this.ErrorMessage = (obj as Variable).identifier + ": " + msg;
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
            return new ErrorExpr(ErrorMessage);
        }
    }

    public class ArgumentError: ErrorExpr
    {
        public ArgumentError(SysFunc func) : this(func, func.Position, func.ValidArguments) { }
        public ArgumentError(FuncStmt func) : this(func, func.Position, func.ValidArguments) { }

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

