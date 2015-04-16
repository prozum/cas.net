using System;

namespace Ast
{
    public enum MsgType {Print, Info, Error};


    public abstract class EvalData
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
    }

    public class PlotData : EvalData
    {
        public Expression func;
        public Symbol sym;

        public PlotData(Expression func, Symbol sym)
        {
            this.sym = sym;
            this.func = func;
        }
    }
}

