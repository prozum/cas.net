using System;

namespace Ast
{
    public class Integer : Number, INegative
    {
        public Int64 value;

        public Integer(Int64 value)
        {
            this.value = value;
        }

        public override string ToString()
        {  
            return value.ToString();
        }

        public override bool CompareTo(Expression other)
        {
            Expression evaluatedOther;

            if (!((evaluatedOther = other.Evaluate()) is Error))
            {
                if (evaluatedOther is Integer)
                {
                    return value == (evaluatedOther as Integer).value;
                }

                if (evaluatedOther is Rational)
                {
                    return value == (evaluatedOther as Rational).value.value;
                }

                if (evaluatedOther is Irrational)
                {
                    return value == (evaluatedOther as Irrational).value;
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

        public bool IsNegative()
        {
            if (value.CompareTo(0) == -1)
            {
                return true;
            }

            return false;
        }

        public Expression ToNegative()
        {
            return new Integer(value * -1);
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

        #region ModuloWith
        public override Expression ModuloWith(Integer other)
        {
            return new Integer(value % other.value);
        }

        public override Expression ModuloWith(Rational other)
        {
            return new Rational(this, new Integer(1)) % other;
        }

        public override Expression ModuloWith(Irrational other)
        {
            return new Irrational((decimal)value % other.value);
        }

        #endregion
    }
}

