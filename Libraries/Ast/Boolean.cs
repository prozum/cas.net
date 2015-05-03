using System;

namespace Ast
{
    public class Boolean : Real
    {
        public bool @bool;

        public Boolean(bool value)
        {
            this.@bool = value;
        }

        public override decimal Value
        {
            get
            {
                return @bool ? 1 : 0;
            }
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


        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Integer((@bool ? 1: 0) + other.@int);
        }

        public override Expression AddWith(Rational other)
        {
            return new Rational(@bool ? 1: 0, 1) + other;
        }

        public override Expression AddWith(Boolean other)
        {
            return new Integer((@bool ? 1: 0) + (@bool ? 1: 0));
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return new Integer((@bool ? 1: 0) - other.@int);
        }

        public override Expression SubWith(Rational other)
        {
            return new Rational((@bool ? 1: 0), 1) - other;
        }

        public override Expression SubWith(Boolean other)
        {
            return new Integer((@bool ? 1: 0) - (other.@bool ? 1: 0));
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return new Integer((@bool ? 1: 0) * other.@int);
        }

        public override Expression MulWith(Rational other)
        {
            return new Rational((@bool ? 1: 0), 1) * other;
        }

        public override Expression MulWith(Boolean other)
        {
            return new Integer((@bool ? 1: 0) * (other.@bool ? 1: 0));
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return new Integer((Int64)Math.Pow((@bool ? 1: 0), other.@int));
        }

        public override Expression ExpWith(Rational other)
        {
            return new Rational((@bool ? 1: 0), 1) ^ other;
        }

        public override Expression ExpWith(Irrational other)
        {
            return new Irrational(Math.Pow((@bool ? 1: 0), (double)other.@decimal));
        }

        #endregion

        #region ModuloWith
        public override Expression ModuloWith(Integer other)
        {
            return new Integer((@bool ? 1: 0) % other.@int);
        }

        public override Expression ModuloWith(Rational other)
        {
            return new Rational((@bool ? 1: 0), 1) % other;
        }

        public override Expression ModuloWith(Irrational other)
        {
            return new Irrational((@bool ? 1: 0) % other.@decimal);
        }

        #endregion
    }
}

