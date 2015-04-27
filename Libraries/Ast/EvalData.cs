using System;

namespace Ast
{
    public enum MsgType {Print, Info, Error};


    public abstract class EvalData
    {
    }

    public class DoneData : EvalData
    {
    }

    public class MsgData : EvalData
    {
        public MsgType type;
        public string msg;

        public MsgData(MsgType type, string msg)
        {
            this.type = type;
            this.msg = msg;
        }

        public override string ToString()
        {
            return msg;
        }
    }

    public class PlotData : EvalData
    {
        public Expression exp;
        public Symbol sym;

        public PlotData(Plot plot)
        {
            this.sym = plot.sym;
            this.exp = plot.exp;
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

