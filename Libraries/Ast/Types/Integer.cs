using System;

namespace Ast
{
    public class Integer : Real
    {
        public Int64 @int;

        public Integer(Int64 value)
        {
            this.@int = value;
        }

        public override decimal @decimal
        {
            get
            {
                return (decimal)@int;
            }
        }

        public static implicit operator Int64(Integer i)
        {
            return i.@int;
        }

        public override Expression Clone(Scope scope)
        {
            return new Integer(@int);
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Integer(@int + other.@int);
        }

        public override Expression AddWith(Rational other)
        {
            return new Rational(@int, 1) + other;
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return new Integer(@int - other.@int);
        }

        public override Expression SubWith(Rational other)
        {
            return new Rational(@int, 1) - other;
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return new Integer(@int * other.@int);
        }

        public override Expression MulWith(Rational other)
        {
            return new Rational(@int, 1) * other;
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return new Integer((Int64)Math.Pow(@int, other.@int));
        }

        public override Expression ExpWith(Rational other)
        {
            return new Rational(@int, 1) ^ other;
        }

        #endregion

        #region ModuloWith
        public override Expression ModWith(Integer other)
        {
            return new Integer(@int % other.@int);
        }

        public override Expression ModWith(Rational other)
        {
            return new Rational(this, new Integer(1)) % other;
        }

        #endregion

    }
}

