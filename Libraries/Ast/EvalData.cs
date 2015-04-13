using System;

namespace Ast
{
    public enum EvalType {Assign, Info, Error, Plot};


    public class EvalData
    {
        public EvalType type;
        public string msg;

        public EvalData(EvalType type, string msg)
        {
            this.type = type;
            this.msg = msg;
        }
    }
}

