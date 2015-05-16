using System;

namespace Ast
{
    public class Rational : Real, INegative
    {
        public Int64 numerator;
        public Int64 denominator;

        public Rational(Int64 num, Int64 denom)
        {
            numerator = num;
            denominator = denom;
        }

        public override decimal Value
        {
            get
            {
                return ((decimal)numerator) / denominator;
            }
        }

        public void Reduce(Integer num, Integer denom)
        {
            Gcd (num, denom);
        }

        public static Integer Gcd(Integer num, Integer denom)
        {
            throw new NotImplementedException ();
        }

        public override Expression Clone()
        {
            return new Rational(numerator, denominator);
        }

        public Expression ToNegative()
        {
            return new Rational(numerator * -1, denominator);
        }

        public override Expression Minus()
        {
            return ToNegative();
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return this + new Rational(other, new Integer(1));
        }

        public override Expression AddWith(Rational other)
        {
            var newNumerator = numerator * other.denominator;
            var otherNewNumerator = denominator * other.numerator;

            return new Rational(newNumerator + otherNewNumerator, denominator * other.denominator);
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return this - new Rational(other, new Integer(1));
        }

        public override Expression SubWith(Rational other)
        {
            var leftNumerator = numerator * other.denominator;
            var rightNumerator = denominator * other.numerator;

            return new Rational(leftNumerator - rightNumerator, denominator * other.denominator);
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return this * new Rational(other, new Integer(1));
        }

        public override Expression MulWith(Rational other)
        {
            return new Rational(numerator * other.numerator, denominator * other.denominator);
        }

        #endregion

        #region DivWith
        public override Expression DivWith(Integer other)
        {
            return this / new Rational(other, new Integer(1));
        }

        public override Expression DivWith(Rational other)
        {
            return this * new Rational(other.denominator, other.numerator);
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return new Rational(numerator ^ other.@int, denominator ^ other.@int);
        }

        #endregion

        #region ModuloWith
        public override Expression ModWith(Integer other)
        {
            return this % new Rational(other, new Integer(1));
        }

        public override Expression ModWith(Rational other)
        {
            var newNumerator = numerator * other.denominator;
            var otherNewNumerator = denominator * other.numerator;

            return new Rational(newNumerator % otherNewNumerator, denominator * other.denominator);
        }

        #endregion
    }
}

