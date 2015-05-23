using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class EvalData
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
        public string msg;
        public Pos Position;

        public ErrorData(string err)
        {
            this.msg = err;
        }

        public ErrorData(Error err)
        {
            this.msg = err.ErrorMessage;
            this.Position = err.Position;
        }

        public ErrorData(Expression expr, string err)
        {
            this.msg = err;
            this.Position = expr.Position;
        }

        public override string ToString()
        {
            var str = "";

            str += "[" + Position.Column;
            str += ";" + Position.Line + "]";
            str += msg;

            return str;
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

