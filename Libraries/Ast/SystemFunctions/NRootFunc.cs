namespace Ast
{
    public class NRootFunc : SysFunc
    {
        public NRootFunc() : this(null) { }
        public NRootFunc(Scope scope)
            : base("nroot", scope)
        {
        }

        public override Expression Call(List args)
        {
            throw new System.NotImplementedException();
        }
    }
}
