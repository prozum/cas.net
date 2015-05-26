namespace Ast
{
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

