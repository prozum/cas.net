using System;

namespace Ast
{
    public class Integer : Real, INegative
    {
        public Int64 @int;

        public Integer(Int64 value)
        {
            this.@int = value;
        }

        public override decimal Value
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

        public override Expression Clone()
        {
            return new Integer(@int);
        }

        public Expression ToNegative()
        {
            return new Integer(@int * -1);
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

        public override Expression AddWith(Boolean other)
        {
            return new Integer(@int + (other.@bool ? 1: 0));
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

        public override Expression SubWith(Boolean other)
        {
            return new Integer(@int - (other.@bool ? 1: 0));
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

        public override Expression MulWith(Boolean other)
        {
            return new Integer(@int * (other.@bool ? 1: 0));
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

        public override Expression ExpWith(Boolean other)
        {
            return new Integer(other.@bool ? @int : 1);
        }

        #endregion

        #region ModuloWith
        public override Expression ModuloWith(Integer other)
        {
            return new Integer(@int % other.@int);
        }

        public override Expression ModuloWith(Rational other)
        {
            return new Rational(this, new Integer(1)) % other;
        }

        #endregion
    }
}

