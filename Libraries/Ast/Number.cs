using System;

namespace Ast
{
    public abstract class Number : Expression 
    {
        public override Expression Evaluate()
        {
            return this;
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
    }
}

