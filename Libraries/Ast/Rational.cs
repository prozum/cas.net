using System;

namespace Ast
{
    public class Rational : Number 
    {
        public Integer numerator;
        public Integer denominator;
        public Irrational value;

        public Rational(Integer num, Integer denom)
        {
            numerator = num;
            denominator = denom;
            value = new Irrational((decimal)num.value / denom.value);
        }

        public override string ToString()
        {
            return value.ToString ();
        }

        public void Reduce(Integer num, Integer denom)
        {
            Gcd (num, denom);
        }

        public static Integer Gcd(Integer num, Integer denom)
        {
            throw new NotImplementedException ();
        }

        public override bool CompareTo(Expression other)
        {
            Expression evaluatedOther;

            if (!((evaluatedOther = other.Evaluate()) is Error))
            {
                if (other is Integer)
                {
                    return value.value == (other as Integer).value;
                }

                if (other is Rational)
                {
                    return value.value == (other as Rational).value.value;
                }

                if (other is Irrational)
                {
                    return value.value == (other as Irrational).value;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public override Expression Clone()
        {
            return new Rational(numerator.Clone() as Integer, denominator.Clone() as Integer);
        }

        public override bool IsNegative()
        {
            return value.IsNegative();
        }

        public override Number ToNegative()
        {
            return new Rational(numerator.ToNegative() as Integer, denominator);
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

            return new Rational((newNumerator + otherNewNumerator) as Integer, (denominator * other.denominator) as Integer);
        }

        public override Expression AddWith(Irrational other)
        {
            return value - other;
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

            return new Rational((leftNumerator - rightNumerator) as Integer, (denominator * other.denominator) as Integer);
        }

        public override Expression SubWith(Irrational other)
        {
            return value - other;
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return this * new Rational(other, new Integer(1));
        }

        public override Expression MulWith(Rational other)
        {
            return new Rational((numerator * other.numerator) as Integer, (denominator * other.denominator) as Integer);
        }

        public override Expression MulWith(Irrational other)
        {
            return value * other;
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

        public override Expression DivWith(Irrational other)
        {
            return value * other;
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return (numerator ^ other) / (denominator ^ other);
        }

        public override Expression ExpWith(Rational other)
        {
            return this ^ other.value;
        }

        public override Expression ExpWith(Irrational other)
        {
            return (numerator ^ other)/(denominator ^ other);
        }

        #endregion

        #region GreaterThan
        public override Expression GreaterThan(Integer other)
        {
            return value > new Rational(other, new Integer(1));
        }

        public override Expression GreaterThan(Rational other)
        {
            return numerator * other.denominator > other.numerator * denominator;
        }

        public override Expression GreaterThan(Irrational other)
        {
            return value > other;
        }

        #endregion

        #region LesserThan
        public override Expression LesserThan(Integer other)
        {
            return value < new Rational(other, new Integer(1));
        }

        public override Expression LesserThan(Rational other)
        {
            return numerator * other.denominator < other.numerator * denominator;
        }

        public override Expression LesserThan(Irrational other)
        {
            return value < other;
        }

        #endregion

        #region GreaterThanOrEqualTo
        public override Expression GreaterThanOrEqualTo(Integer other)
        {
            return value >= new Rational(other, new Integer(1));
        }

        public override Expression GreaterThanOrEqualTo(Rational other)
        {
            return numerator * other.denominator >= other.numerator * denominator;
        }

        public override Expression GreaterThanOrEqualTo(Irrational other)
        {
            return value >= other;
        }

        #endregion

        #region LesserThanOrEqualTo
        public override Expression LesserThanOrEqualTo(Integer other)
        {
            return value <= new Rational(other, new Integer(1));
        }

        public override Expression LesserThanOrEqualTo(Rational other)
        {
            return numerator * other.denominator <= other.numerator * denominator;
        }

        public override Expression LesserThanOrEqualTo(Irrational other)
        {
            return value <= other;
        }

        #endregion
    }
}

