using System;

namespace Ast
{
    public abstract class EvalData
    {
    }

    public class DoneData : EvalData
    {
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
        public string err;

        public ErrorData(string err)
        {
            this.err = err;
        }

        public ErrorData(Error err)
        {
            this.err = err.msg;
        }

        public override string ToString()
        {
            return err;
        }
    }

    public class DebugData : EvalData
    {
        public string msg;
        public Expression expr;

        public DebugData(string msg, Expression expr)
        {
            this.msg = msg;
            this.expr = expr;
        }

        public override string ToString()
        {
            return msg + expr.ToString();
        }
    }
}

