namespace Ast
{
    public class Minus : PrefixOperator
    {
        public Minus() : base("-") { }

        public override Expression Evaluate()
        {
            return Child.Evaluate().Minus();
        }

        public override Expression Reduce()
        {
            var res = new Minus();
            res.Child = Child.Reduce();
            if (res.Child is Variable)
                return (res.Child as Variable).ToNegative();

            return res;
        }
    }
}

