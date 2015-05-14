using System;

namespace Ast
{
    public class Text : Expression
    {
        public string Value;

        public Text(string value = "")
        {
            this.Value = value;
        }

        public static implicit operator string(Text t)
        {
            return t.Value;
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
            return "\"" + Value + "\"";
        }

        public override Expression AddWith(Text other)
        {
            return new Text(this.Value + other);
        }

        public override Expression AddWith(Integer other)
        {
            return new Text(this.Value + other.ToString());
        }

        public override Expression AddWith(Rational other)
        {
            return new Text(this.Value + other.ToString());
        }

        public override Expression AddWith(Irrational other)
        {
            return new Text(this.Value + other.ToString());
        }

        public override Expression AddWith(Complex other)
        {
            return new Text(this.Value + other.ToString());
        }
    }
}

