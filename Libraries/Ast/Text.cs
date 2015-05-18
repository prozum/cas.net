using System;

namespace Ast
{
    public class Text : Expression
    {
        public string @string;

        public Text(string value = "")
        {
            @string = value;
        }

        public static implicit operator string(Text t)
        {
            return t.@string;
        }

        public override Expression Evaluate()
        {
            return this;
        }

        public override string ToString()
        {
            return "\"" + @string + "\"";
        }

        public override bool CompareTo(Expression other)
        {
            return false;
        }

        public override Expression AddWith(Text other)
        {
            return new Text(@string + other);
        }

        public override Expression AddWith(Integer other)
        {
            return new Text(@string + other.ToString());
        }

        public override Expression AddWith(Rational other)
        {
            return new Text(@string + other.ToString());
        }

        public override Expression AddWith(Irrational other)
        {
            return new Text(@string + other.ToString());
        }

        public override Expression AddWith(Complex other)
        {
            return new Text(@string + other.ToString());
        }
    }
}

