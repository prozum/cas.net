namespace Ast
{
    public class Referation : PrefixOperator
    {
        public Referation() : base("~") { }

        public override Expression Value
        {
            get
            {
                return Child.Value;
            }
            set
            {
                Child.Value = value;
            }
        }

        public override Expression Evaluate()
        {
            return Child;
        }

        public override Expression Clone(Scope scope)
        {
            var refe = new Referation();
            refe.CurScope = scope;
            refe.Child = Child.Clone(scope);

            return refe;
        }
    }
}

