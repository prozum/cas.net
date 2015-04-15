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

        public virtual Expression AddWith(Expression other)
        {
            if (other is Error)
            {
                return other;
            }

            return AddWith(other.Evaluate());
        }

        public virtual Expression SubWith(Expression other)
        {
            if (other is Error)
            {
                return other;
            }

            return SubWith(other.Evaluate());
        }

        public virtual Expression MulWith(Expression other)
        {
            if (other is Error)
            {
                return other;
            }

            return MulWith(other.Evaluate());
        }

        public virtual Expression DivWith(Expression other)
        {
            if (other is Error)
            {
                return other;
            }

            return DivWith(other.Evaluate());
        }

        public virtual Expression ExpWith(Expression other)
        {
            if (other is Error)
            {
                return other;
            }

            return ExpWith(other.Evaluate());
        }

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

        public override Expression AddWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Integer(value + (other as Integer).value);
                }

                if (other is Rational)
                {
                    return new Rational(this, new Integer(1)).AddWith(other);
                }

                if (other is Irrational)
                {
                    return new Irrational(value + (other as Irrational).value);
                }
            }

            return base.AddWith(other);
        }

        public override Expression SubWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Integer(value - (other as Integer).value);
                }

                if (other is Rational)
                {
                    return new Rational(this, new Integer(1)).SubWith(other);
                }

                if (other is Irrational)
                {
                    return new Irrational(value - (other as Irrational).value);
                }
            }

            return base.SubWith(other);
        }

        public override Expression MulWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Integer(value * (other as Integer).value);
                }

                if (other is Rational)
                {
                    return new Rational(this, new Integer(1)).MulWith(other);
                }

                if (other is Irrational)
                {
                    return new Irrational(value * (other as Irrational).value);
                }
            }

            return base.MulWith(other);
        }

        public override Expression DivWith(Expression other)
        {
            if (other is Number)
            {
                if (other.CompareTo(new Integer(0)))
                {
                    return new Error(this ,"Diving by 0 not allowed");
                }

                if (other is Integer)
                {
                    return new Rational(this, (other as Integer));
                }

                if (other is Rational)
                {
                    return new Rational(this, new Integer(1)).DivWith(other);
                }

                if (other is Irrational)
                {
                    return new Irrational(value / (other as Irrational).value);
                }
            }

            return base.DivWith(other);
        }

        public override Expression ExpWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Irrational((decimal)Math.Pow(value, (other as Integer).value));
                }

                if (other is Rational)
                {
                    return new Rational(this, new Integer(1)).ExpWith(other);
                }

                if (other is Irrational)
                {
                    return new Irrational((decimal)Math.Pow(value, (double)(other as Irrational).value));
                }
            }

            return base.ExpWith(other);
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

        public override Number Clone()
        {
            return new Rational(numerator.Clone() as Integer, denominator.Clone() as Integer);
        }

        public override Expression AddWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return AddWith(new Rational(other as Integer, new Integer(1)));
                }

                if (other is Rational)
                {
                    Integer leftNumerator = numerator.MulWith((other as Rational).denominator) as Integer;
                    Integer rightNumerator = denominator.MulWith((other as Rational).numerator) as Integer;

                    return new Rational(leftNumerator.AddWith(rightNumerator) as Integer, denominator.MulWith((other as Rational).denominator)as Integer);
                }

                if (other is Irrational)
                {
                    return new Irrational(value.value + (other as Irrational).value);
                }
            }

            return base.AddWith(other);
        }

        public override Expression SubWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return SubWith(new Rational(other as Integer, new Integer(1)));
                }

                if (other is Rational)
                {
                    Integer leftNumerator = numerator.MulWith((other as Rational).denominator) as Integer;
                    Integer rightNumerator = denominator.MulWith((other as Rational).numerator) as Integer;

                    return new Rational(leftNumerator.SubWith(rightNumerator) as Integer, denominator.MulWith((other as Rational).denominator) as Integer);
                }

                if (other is Irrational)
                {
                    return new Irrational(value.value - (other as Irrational).value);
                }
            }

            return base.SubWith(other);
        }

        public override Expression MulWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return MulWith(new Rational(other as Integer, new Integer(1)));
                }

                if (other is Rational)
                {
                    return new Rational(numerator.MulWith((other as Rational).numerator) as Integer, denominator.MulWith((other as Rational).denominator) as Integer);
                }

                if (other is Irrational)
                {
                    return new Irrational(value.value * (other as Irrational).value);
                }
            }

            return base.MulWith(other);
        }

        public override Expression DivWith(Expression other)
        {
            if (other is Number)
            {
                if (other.CompareTo(new Integer(0)))
                {
                    return new Error(this, "Diving by 0 not allowed");
                }

                if (other is Integer)
                {
                    return DivWith(new Rational(other as Integer, new Integer(1)));
                }

                if (other is Rational)
                {
                    return new Rational(denominator, numerator).MulWith(other);
                }

                if (other is Irrational)
                {
                    return new Irrational(value.value / (other as Irrational).value);
                }
            }

            return base.DivWith(other);
        }

        public override Expression ExpWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Rational(numerator.ExpWith(other) as Integer, denominator.ExpWith(other) as Integer);
                }

                if (other is Rational)
                {
                    return ExpWith((other as Rational).value);
                }

                if (other is Irrational)
                {
                    return ((numerator.ExpWith(other)) as Number).DivWith(denominator.ExpWith(other));
                }
            }

            return base.ExpWith(other);
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

        public override Number Clone()
        {
            return new Irrational(value);
        }

        public override Expression AddWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Irrational(value + (other as Integer).value);
                }

                if (other is Rational)
                {
                    return AddWith((other as Rational).value);
                }

                if (other is Irrational)
                {
                    return new Irrational(value + (other as Irrational).value);
                }
            }

            return base.AddWith(other);
        }

        public override Expression SubWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Irrational(value - (other as Integer).value);
                }

                if (other is Rational)
                {
                    return SubWith((other as Rational).value);
                }

                if (other is Irrational)
                {
                    return new Irrational(value - (other as Irrational).value);
                }
            }

            return base.SubWith(other);
        }

        public override Expression MulWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Irrational(value * (other as Integer).value);
                }

                if (other is Rational)
                {
                    return MulWith((other as Rational).value);
                }

                if (other is Irrational)
                {
                    return new Irrational(value * (other as Irrational).value);
                }
            }

            return base.MulWith(other);
        }

        public override Expression DivWith(Expression other)
        {
            if (other is Number)
            {
                if (other.CompareTo(new Integer(0)))
                {
                    return new Error(this, "Diving by 0 not allowed");
                }

                if (other is Integer)
                {
                    return new Irrational(value / (other as Integer).value);
                }

                if (other is Rational)
                {
                    return DivWith((other as Rational).value);
                }

                if (other is Irrational)
                {
                    return new Irrational(value / (other as Irrational).value);
                }
            }

            return base.DivWith(other);
        }

        public override Expression ExpWith(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    return new Irrational((decimal)Math.Pow((double)value, (other as Integer).value));
                }

                if (other is Rational)
                {
                    return ExpWith((other as Rational).value);
                }

                if (other is Irrational)
                {
                    return new Irrational((decimal)Math.Pow((double)value, (double)(other as Irrational).value));
                }
            }

            return base.ExpWith(other);
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

