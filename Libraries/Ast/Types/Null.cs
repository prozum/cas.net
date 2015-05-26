namespace Ast
{
    public class Null : Expression
    {
        public Null()
        {
        }

        public override string ToString()
        {
            return "null";
        }

        public override Expression Evaluate()
        {
            return this;
        }
    }
}