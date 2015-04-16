using System;

namespace Ast
{
    public abstract class Number : Expression 
    {
        public override Expression Evaluate()
        {
            return this;
        }

        public abstract Number Clone();

        public override bool ContainsNotNumber(NotNumber other)
        {
            return false;
        }
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

        public override Number Clone()
        {
            return new Integer(value);
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Integer(value + other.value);
        }

        public override Expression AddWith(Rational other)
        {
            return new Rational(this, new Integer(1)) - other;
        }

        public override Expression AddWith(Irrational other)
        {
            return new Irrational(value + other.value);
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
            return new Irrational(value - other.value);
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
            return new Irrational(value * other.value);
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
            return new Irrational(value / other.value);
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

        /*
        #region Operator Overloads
        #region Add Overload
        public static Integer operator +(Integer left, Integer right)
        {
            return new Integer(left.value + right.value);
        }

        public static Rational operator +(Integer left, Rational right)
        {
            return new Rational(left, new Integer(1)) + right;
        }

        public static Rational operator +(Rational left, Integer right)
        {
            return left + new Rational(right, new Integer(1));
        }

        public static Irrational operator +(Integer left, Irrational right)
        {
            return new Irrational(left.value + right.value);
        }

        public static Irrational operator +(Irrational left, Integer right)
        {
            return new Irrational(left.value + right.value);
        }
        #endregion

        #region Sub Overload
        public static Integer operator -(Integer left, Integer right)
        {
            return new Integer(left.value - right.value);
        }

        public static Rational operator -(Integer left, Rational right)
        {
            return new Rational(left, new Integer(1)) - right;
        }

        public static Rational operator -(Rational left, Integer right)
        {
            return left + new Rational(right, new Integer(1));
        }

        public static Irrational operator -(Integer left, Irrational right)
        {
            return new Irrational(left.value - right.value);
        }

        public static Irrational operator -(Irrational left, Integer right)
        {
            return new Irrational(left.value - right.value);
        }
        #endregion

        #region Mul Overload
        public static Integer operator *(Integer left, Integer right)
        {
            return new Integer(left.value * right.value);
        }

        public static Rational operator *(Integer left, Rational right)
        {
            return new Rational(left, new Integer(1)) * right;
        }

        public static Rational operator *(Rational left, Integer right)
        {
            return left * new Rational(right, new Integer(1));
        }

        public static Irrational operator *(Integer left, Irrational right)
        {
            return new Irrational(left.value * right.value);
        }

        public static Irrational operator *(Irrational left, Integer right)
        {
            return new Irrational(left.value * right.value);
        }
        #endregion

        #region Div Overload
        public static Integer operator /(Integer left, Integer right)
        {
            return new Integer(left.value * right.value);
        }

        public static Rational operator /(Integer left, Rational right)
        {
            return new Rational(left, new Integer(1)) / right;
        }

        public static Rational operator /(Rational left, Integer right)
        {
            return left / new Rational(right, new Integer(1));
        }

        public static Irrational operator /(Integer left, Irrational right)
        {
            return new Irrational(left.value / right.value);
        }

        public static Irrational operator /(Irrational left, Integer right)
        {
            return new Irrational(left.value / right.value);
        }
        #endregion

        #region Exp Overload
        public static Integer operator ^(Integer left, Integer right)
        {
            return new Integer((int)Math.Pow(left.value, right.value));
        }

        public static Irrational operator ^(Integer left, Rational right)
        {
            return left ^ right.value;
        }

        public static Rational operator ^(Rational left, Integer right)
        {
            return new Rational(left.numerator ^ right, left.denominator ^ right);
        }

        public static Irrational operator ^(Integer left, Irrational right)
        {
            return new Irrational((decimal)Math.Pow(left.value, (double)right.value));
        }

        public static Irrational operator ^(Irrational left, Integer right)
        {
            return new Irrational((decimal)Math.Pow((double)left.value, right.value));
        }
        #endregion

        #region LessThan Overload

        #endregion

        #region LessThanOrEqual Overload

        #endregion

        #region GreaterThan Overload

        #endregion

        #region GreaterThanOrEqual Overload

        #endregion

        #region Equal

        #endregion
        #endregion
        */
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

        public override Number Clone()
        {
            return new Rational(numerator.Clone() as Integer, denominator.Clone() as Integer);
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return this + new Rational(other, new Integer(1));
        }

        public override Expression AddWith(Rational other)
        {
            var leftNumerator = numerator * other.denominator;
            var rightNumerator = denominator * other.numerator;

            return new Rational((leftNumerator + rightNumerator) as Integer, (denominator * other.denominator) as Integer);
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
            return new Rational(denominator, numerator) * other;
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

        /*
        #region Operator Overloads
        #region Add Overload
        public static Rational operator +(Rational left, Rational right)
        {
            var leftNumerator = left.numerator * right.denominator;
            var rightNumerator = right.numerator * left.denominator;

            return new Rational(leftNumerator + rightNumerator, right.denominator * left.denominator);
        }

        public static Irrational operator +(Rational left, Irrational right)
        {
            return new Irrational(left.value.value + right.value);
        }

        public static Irrational operator +(Irrational left, Rational right)
        {
            return new Irrational(left.value + right.value.value);
        }
        #endregion

        #region Sub Overload
        public static Rational operator -(Rational left, Rational right)
        {
            var leftNumerator = left.numerator * right.denominator;
            var rightNumerator = right.numerator * left.denominator;

            return new Rational(leftNumerator - rightNumerator, right.denominator * left.denominator);
        }

        public static Irrational operator -(Rational left, Irrational right)
        {
            return new Irrational(left.value.value - right.value);
        }

        public static Irrational operator -(Irrational left, Rational right)
        {
            return new Irrational(left.value - right.value.value);
        }
        #endregion

        #region Mul Overload
        public static Rational operator *(Rational left, Rational right)
        {
            return new Rational(left.numerator * right.numerator, left.denominator * right.denominator);
        }

        public static Irrational operator *(Rational left, Irrational right)
        {
            return new Irrational(left.value.value * right.value);
        }

        public static Irrational operator *(Irrational left, Rational right)
        {
            return new Irrational(left.value * right.value.value);
        }
        #endregion

        #region Div Overload
        public static Rational operator /(Rational left, Rational right)
        {
            return new Rational(left.denominator, left.numerator) * right;
        }

        public static Irrational operator /(Rational left, Irrational right)
        {
            return new Irrational(left.value.value / right.value);
        }

        public static Irrational operator /(Irrational left, Rational right)
        {
            return new Irrational(left.value / right.value.value);
        }
        #endregion

        #region Exp Overload
        public static Irrational operator ^(Rational left, Rational right)
        {
            return left.value ^ right.value;
        }

        public static Irrational operator ^(Rational left, Irrational right)
        {
            return left.value ^ right;
        }

        public static Irrational operator ^(Irrational left, Rational right)
        {
            return left ^ right.value;
        }
        #endregion

        #region LessThan Overload

        #endregion

        #region LessThanOrEqual Overload

        #endregion

        #region GreaterThan Overload

        #endregion

        #region GreaterThanOrEqual Overload

        #endregion

        #region Equal

        #endregion
        #endregion
        */
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

        public override Number Clone()
        {
            return new Irrational(value);
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Irrational(value + other.value);
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
            return new Irrational(value - other.value);
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
            return new Irrational(value * other.value);
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
            return new Irrational(value / other.value);
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

        /*
        #region Operator Overloads
        #region Add Overload
        public static Irrational operator +(Irrational left, Irrational right)
        {
            return new Irrational(left.value + right.value);
        }
        #endregion

        #region Sub Overload
        public static Irrational operator -(Irrational left, Irrational right)
        {
            return new Irrational(left.value - right.value);
        }
        #endregion

        #region Mul Overload
        public static Irrational operator *(Irrational left, Irrational right)
        {
            return new Irrational(left.value * right.value);
        }
        #endregion

        #region Div Overload
        public static Irrational operator /(Irrational left, Irrational right)
        {
            return new Irrational(left.value / right.value);
        }
        #endregion

        #region Exp Overload
        public static Irrational operator ^(Irrational left, Irrational right)
        {
            return new Irrational((decimal)Math.Pow((double)left.value, (double)right.value));
        }
        #endregion

        #region LessThan Overload

        #endregion

        #region LessThanOrEqual Overload

        #endregion

        #region GreaterThan Overload

        #endregion

        #region GreaterThanOrEqual Overload

        #endregion

        #region Equal

        #endregion
        #endregion
        */
    }

    public class Complex : Number 
    {
        public Number real;
        public Number imag;

        public override string ToString()
        {
            return real.ToString () + '+' + imag.ToString() + 'i';
        }

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (real == (other as Complex).real && real == (imag as Complex).imag)
                {
                    res = true;
                }
            }

            return res;
        }

        public override Number Clone()
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

        public override Number Clone()
        {
            return new Boolean(value);
        }
    }
}

