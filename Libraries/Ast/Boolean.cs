using System;

namespace Ast
{
    public class Boolean : Expression
    {
        public bool @bool;

        public Boolean(bool value)
        {
            this.@bool = value;
        }

        public static bool operator true (Boolean b)
        {
            return b.@bool;
        }

        public static bool operator false (Boolean b)
        {
            return b.@bool;
        }

        public override string ToString()
        {
            return @bool.ToString();
        }

        public override Expression Clone()
        {
            return new Boolean(@bool);
        }

        public override Expression Negation()
        {
            return new Boolean(!@bool);
        }
    }
}

