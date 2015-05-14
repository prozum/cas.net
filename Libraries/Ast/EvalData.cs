using System;

namespace Ast
{
    public abstract class EvalData
    {
    }

    public class DoneData : EvalData
    {
        public Expression expr;

        public DoneData(Expression expr = null)
        {
            this.expr = expr;
        }
    }

    public class ReturnData : EvalData
    {
        public Expression expr;

        public ReturnData(Expression expr)
        {
            this.expr = expr;
        }

        public override string ToString()
        {
            return expr.ToString();
        }
    }

    public class PrintData : EvalData
    {
        public string msg;

        public PrintData(string msg)
        {
            this.msg = msg;
        }

        public override string ToString()
        {
            return msg;
        }
    }

    public class ErrorData : EvalData
    {
        public string msg;
        public Pos pos;

        public ErrorData(string err)
        {
            this.msg = err;
        }

        public ErrorData(Error err)
        {
            this.msg = err.ErrorMessage;
            this.pos = err.Position;
        }

        public ErrorData(Expression expr, string err)
        {
            this.msg = err;
            this.pos = expr.Position;
        }

        public ErrorData(Statement stmt, string err)
        {
            this.msg = err;
            this.pos = stmt.Position;
        }

        public override string ToString()
        {
            var str = "";

            str += "[" + pos.Column;
            str += ";" + pos.Line + "]";
            str += msg;

            return str;
        }
    }

    public class ExprData : EvalData
    {
        public Expression expr;

        public ExprData(Expression expr)
        {
            this.expr = expr;
        }

        public override string ToString()
        {
            return expr.ToString();
        }
    }


    public class DebugData : EvalData
    {
        public string msg;

        public DebugData(string msg)
        {
            this.msg = msg;
        }

        public override string ToString()
        {
            return msg;
        }
    }
}

