using System;

namespace Ast
{
    public class Text : Expression
    {
        public string value;

        public Text(string value = "")
        {
            this.value = value;
        }

        public override bool ContainsVariable(Variable other)
        {
            return false;
        }

        public override Expression Evaluate()
        {
            return this;
        }

        public override string ToString()
        {
            return value;
        }

        public override Expression AddWith(Text other)
        {
            return new Text(this.value + other.value);
        }

        public override Expression AddWith(Integer other)
        {
            return new Text(this.value + other.ToString());
        }

        public override Expression AddWith(Rational other)
        {
            return new Text(this.value + other.ToString());
        }

        public override Expression AddWith(Irrational other)
        {
            return new Text(this.value + other.ToString());
        }

        public override Expression AddWith(Complex other)
        {
            return new Text(this.value + other.ToString());
        }
    }
}

