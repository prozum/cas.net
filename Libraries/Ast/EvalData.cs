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
        public Symbol sym;
        public Expression func;

        public PlotData(Symbol sym, Expression func)
        {
            this.sym = sym;
            this.func = func;
        }
    }
}

