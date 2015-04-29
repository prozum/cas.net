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
    }

    public class ExprData : EvalData
    {
        public Expression expr;

        public ExprData(Expression exp)
        {
            this.expr = exp;
        }

        public override string ToString()
        {
            return expr.ToString();
        }
    }
}

