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

        public override Expression Clone(Scope scope)
        {
            return new Text(@string);
        }

        public override string ToString()
        {
            return @string;
        }

        public override bool CompareTo(Expression other)
        {
            if (other is Text)
            {
                return (@string.CompareTo((other as Text).@string) == 0) ? true : false;
            }
            if (other is TypeFunc)
            {
                var text = (other as TypeFunc).Evaluate();

                if (text is Text)
                {
                    return (@string.CompareTo((text as Text).@string) == 0) ? true : false;
                }
            }

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

        public override Expression AddWith(Boolean other)
        {
            return new Text(@string + other.ToString());
        }
    }
}

