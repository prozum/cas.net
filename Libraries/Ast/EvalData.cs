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

    public class ExpData : EvalData
    {
        public Expression exp;

        public ExpData(Expression exp)
        {
            this.exp = exp;
        }

        public override string ToString()
        {
            return exp.ToString();
        }
    }
}

