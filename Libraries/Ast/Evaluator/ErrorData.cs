using System;

namespace Ast
{
    public class ErrorData : EvalData
    {
        public string ErrorMessage;
        public Pos Position;

        public ErrorData(string msg)
        {
            ErrorMessage = msg;
        }

        public ErrorData(Expression expr, string msg)
        {
            if (expr is Variable)
                ErrorMessage = (expr as Variable).Identifier + ": " + msg;
            else
                ErrorMessage = expr.GetType().Name + ": " + msg;
                
            Position = expr.Position;
        }

        public ErrorData(Pos pos, string msg)
        {
            Position = pos;
            ErrorMessage = msg;
        }

        public override string ToString()
        {
            var str = "";

            str += "[" + Position.Column;
            str += ";" + Position.Line + "]";
            str += ErrorMessage;

            return str;
        }
    }

    public class ArgErrorData: ErrorData
    {
        public ArgErrorData(SysFunc func) : base(func, "Valid arguments: ")
        {
            ErrorMessage += "[";
            for(int i = 0; i < func.ValidArguments.Count; i++)
            {
                ErrorMessage += func.ValidArguments[i].ToString();

                if (i < func.ValidArguments.Count -1) 
                {
                    ErrorMessage += ',';
                }
            }
            ErrorMessage += "]";
        }
    }
}

