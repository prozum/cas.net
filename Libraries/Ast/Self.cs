namespace Ast
{
    public class Self : Expression
    {
        public Self(Scope scope)
        {
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            return Value;
        }

        public override Expression Value
        {
            get
            {
                return CurScope;
            }
        }

        public override Expression Clone(Scope scope)
        {
            return new Self(scope);
        }

        public override string ToString()
        {
            return "self";
        }
    }
}

