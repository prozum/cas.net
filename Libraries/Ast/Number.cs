using System;

namespace Ast
{
    public abstract class Number : Expression 
    {
        public override Expression Evaluate()
        {
            return this;
        }

        public override bool ContainsVariable(Variable other)
        {
            return false;
        }

        public abstract bool IsNegative();

        public abstract void ToNegative();
    }

    public class Integer : Number
    {
        public Int64 value;

        public Integer(Int64 value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString ();
        }

        public override bool CompareTo(Expression other)
        {
            Expression evaluatedOther;

            if (!((evaluatedOther = other.Evaluate()) is Error))
            {
                if (other is Integer)
                {
                    return value == (other as Integer).value;
                }

                if (other is Rational)
                {
                    return value == (other as Rational).value.value;
                }

                if (other is Irrational)
                {
                    return value == (other as Irrational).value;
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
            return new Integer(value);
        }

        public override bool IsNegative()
        {
            if (value.CompareTo(0) == -1)
            {
                return true;
            }

            return false;
        }

        public override void ToNegative()
        {
            value *= -1;
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Integer(value + other.value);
        }

        public override Expression AddWith(Rational other)
        {
            return new Rational(this, new Integer(1)) + other;
        }

        public override Expression AddWith(Irrational other)
        {
            return new Irrational((decimal)value + other.value);
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return new Integer(value - other.value);
        }

        public override Expression SubWith(Rational other)
        {
            return new Rational(this, new Integer(1)) - other;
        }

        public override Expression SubWith(Irrational other)
        {
            return new Irrational((decimal)value - other.value);
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return new Integer(value * other.value);
        }

        public override Expression MulWith(Rational other)
        {
            return new Rational(this, new Integer(1)) * other;
        }

        public override Expression MulWith(Irrational other)
        {
            return new Irrational((decimal)value * other.value);
        }

        #endregion

        #region DivWith
        public override Expression DivWith(Integer other)
        {
            return new Rational(this, other);
        }

        public override Expression DivWith(Rational other)
        {
            return new Rational(this, new Integer(1)) / other;
        }

        public override Expression DivWith(Irrational other)
        {
            return new Irrational((decimal)value / other.value);
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return new Irrational((decimal)Math.Pow(value, other.value));
        }

        public override Expression ExpWith(Rational other)
        {
            return new Rational(this, new Integer(1)) ^ other;
        }

        public override Expression ExpWith(Irrational other)
        {
            return new Irrational((decimal)Math.Pow(value, (double)other.value));
        }

        #endregion

        #region GreaterThan
        public override Expression GreaterThan(Integer other)
        {
            return new Boolean(value > other.value);
        }

        public override Expression GreaterThan(Rational other)
        {
            return new Rational(this, new Integer(1)) > other;
        }

        public override Expression GreaterThan(Irrational other)
        {
            return new Boolean((decimal)value > other.value);
        }

        #endregion

        #region LesserThan
        public override Expression LesserThan(Integer other)
        {
            return new Boolean(value < other.value);
        }

        public override Expression LesserThan(Rational other)
        {
            return new Rational(this, new Integer(1)) < other;
        }

        public override Expression LesserThan(Irrational other)
        {
            return new Boolean((decimal)value < other.value);
        }

        #endregion

        #region GreaterThanOrEqualTo
        public override Expression GreaterThanOrEqualTo(Integer other)
        {
            return new Boolean(value >= other.value);
        }

        public override Expression GreaterThanOrEqualTo(Rational other)
        {
            return new Rational(this, new Integer(1)) >= other;
        }

        public override Expression GreaterThanOrEqualTo(Irrational other)
        {
            return new Boolean((decimal)value >= other.value);
        }

        #endregion

        #region LesserThanOrEqualTo
        public override Expression LesserThanOrEqualTo(Integer other)
        {
            return new Boolean(value <= other.value);
        }

        public override Expression LesserThanOrEqualTo(Rational other)
        {
            return new Rational(this, new Integer(1)) <= other;
        }

        public override Expression LesserThanOrEqualTo(Irrational other)
        {
            return new Boolean((decimal)value <= other.value);
        }

        #endregion
    }

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

        public override void ToNegative()
        {
            numerator.ToNegative();
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

    public class Irrational : Number 
    {
        public decimal value;

        public Irrational(decimal value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString ();
        }

        public override bool CompareTo(Expression other)
        {
            Expression evaluatedOther;

            if (!((evaluatedOther = other.Evaluate()) is Error))
            {
                if (other is Integer)
                {
                    return value == (other as Integer).value;
                }

                if (other is Rational)
                {
                    return value == (other as Rational).value.value;
                }

                if (other is Irrational)
                {
                    return value == (other as Irrational).value;
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
            return new Irrational(value);
        }

        public override bool IsNegative()
        {
            if (value.CompareTo(0) == -1)
            {
                return true;
            }

            return false;
        }

        public override void ToNegative()
        {
            value *= -1;
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Irrational(value + (decimal)other.value);
        }

        public override Expression AddWith(Rational other)
        {
            return this + other.value;
        }

        public override Expression AddWith(Irrational other)
        {
            return new Irrational(value + other.value);
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return new Irrational(value - (decimal)other.value);
        }

        public override Expression SubWith(Rational other)
        {
            return this - other.value;
        }

        public override Expression SubWith(Irrational other)
        {
            return new Irrational(value - other.value);
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return new Irrational(value * (decimal)other.value);
        }

        public override Expression MulWith(Rational other)
        {
            return this * other.value;
        }

        public override Expression MulWith(Irrational other)
        {
            return new Irrational(value * other.value);
        }

        #endregion

        #region DivWith
        public override Expression DivWith(Integer other)
        {
            return new Irrational(value / (decimal)other.value);
        }

        public override Expression DivWith(Rational other)
        {
            return this / other.value;
        }

        public override Expression DivWith(Irrational other)
        {
            return new Irrational(value / other.value);
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return new Irrational((decimal)Math.Pow((double)value, other.value));
        }

        public override Expression ExpWith(Rational other)
        {
            return this ^ other.value;
        }

        public override Expression ExpWith(Irrational other)
        {
            return new Irrational((decimal)Math.Pow((double)value, (double)other.value));
        }

        #endregion

        #region GreaterThan
        public override Expression GreaterThan(Integer other)
        {
            return new Boolean(value > (decimal)other.value);
        }

        public override Expression GreaterThan(Rational other)
        {
            return new Boolean(value > other.value.value);
        }

        public override Expression GreaterThan(Irrational other)
        {
            return new Boolean(value > other.value);
        }

        #endregion

        #region LesserThan
        public override Expression LesserThan(Integer other)
        {
            return new Boolean(value < (decimal)other.value);
        }

        public override Expression LesserThan(Rational other)
        {
            return new Boolean(value < other.value.value);
        }

        public override Expression LesserThan(Irrational other)
        {
            return new Boolean(value < other.value);
        }

        #endregion

        #region GreaterThanEqualTo
        public override Expression GreaterThanOrEqualTo(Integer other)
        {
            return new Boolean(value >= (decimal)other.value);
        }

        public override Expression GreaterThanOrEqualTo(Rational other)
        {
            return new Boolean(value >= other.value.value);
        }

        public override Expression GreaterThanOrEqualTo(Irrational other)
        {
            return new Boolean(value >= other.value);
        }

        #endregion

        #region LesserThanOrEqualTo
        public override Expression LesserThanOrEqualTo(Integer other)
        {
            return new Boolean(value <= (decimal)other.value);
        }

        public override Expression LesserThanOrEqualTo(Rational other)
        {
            return new Boolean(value <= other.value.value);
        }

        public override Expression LesserThanOrEqualTo(Irrational other)
        {
            return new Boolean(value <= other.value);
        }

        #endregion
    }

    public class Complex : Number 
    {
        public Number real;
        public Number imag;

        public Complex(Number real, Number imag)
        {
            this.real = real;
            this.imag = imag;
        }

        public override string ToString()
        {
            return real.ToString () + '+' + imag.ToString() + 'i';
        }

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (!real.CompareTo((other as Complex).real) || !imag.CompareTo((imag as Complex).imag))
                {
                    res = false;
                }
            }

            return res;
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }

        public override bool IsNegative()
        {
            throw new NotImplementedException();
        }

        public override void ToNegative()
        {
            throw new NotImplementedException();
        }
    }

    public class Boolean : Number
    {
        public bool value;

        public Boolean(bool value)
        {
            this.value = value;
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

        public override void ToNegative()
        {
            throw new NotImplementedException();
        }
    }
}

