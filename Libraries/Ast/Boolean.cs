using System;

namespace Ast
{
    public class Boolean : Number
    {
        public bool value;

        public Boolean(bool value)
        {
            this.value = value;
        }

        public static bool operator true (Boolean b)
        {
            return b.value;
        }

        public static bool operator false (Boolean b)
        {
            return b.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override bool CompareTo(Expression other)
        {
            return base.CompareTo(other) && value == (other as Boolean).value;
        }

        public override Expression Clone()
        {
            return new Boolean(value);
        }

        public override bool IsNegative()
        {
            throw new NotImplementedException();
        }

        public override Number ToNegative()
        {
            throw new NotImplementedException();
        }
    }
}

