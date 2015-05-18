using System;

namespace Ast
{
    public abstract class Real : Number
    {
        public abstract Decimal @decimal
        {
            get;
        }
            
        public static implicit operator Decimal(Real r)
        {
            return r.@decimal;
        }

        public static implicit operator double(Real r)
        {
            return (Double)r.@decimal;
        }

        public override string ToString()
        {
            return @decimal.ToString();
        }

        public override bool CompareTo(Expression other)
        {
            Expression evalOther = other.Evaluate();

            if (evalOther is Real)
            {
                return @decimal == evalOther as Real;
            }

            return false;
        }

        public bool IsNegative()
        {
            return @decimal < 0;
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Irrational(@decimal + other.@decimal);
        }

        public override Expression AddWith(Rational other)
        {
            return new Irrational(@decimal + other.@decimal);
        }

        public override Expression AddWith(Irrational other)
        {
            return new Irrational(@decimal + other.@decimal);
        }

        public override Expression AddWith(Text other)
        {
            return new Text(@decimal + other.@string);
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return new Irrational(@decimal - other.@decimal);
        }

        public override Expression SubWith(Rational other)
        {
            return new Irrational(@decimal - other.@decimal);
        }

        public override Expression SubWith(Irrational other)
        {
            return new Irrational(@decimal - other.@decimal);
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return new Irrational(@decimal * other.@decimal);
        }

        public override Expression MulWith(Rational other)
        {
            return new Irrational(@decimal * other.@decimal);
        }

        public override Expression MulWith(Irrational other)
        {
            return new Irrational(@decimal * other.@decimal);
        }

        #endregion

        #region DivWith
        public override Expression DivWith(Integer other)
        {
            return new Irrational(@decimal / other.@decimal);
        }

        public override Expression DivWith(Rational other)
        {
            return new Irrational(@decimal / other.@decimal);
        }

        public override Expression DivWith(Irrational other)
        {
            return new Irrational(@decimal / other.@decimal);
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return new Irrational(Math.Pow((double)@decimal, (double)other.@decimal));
        }

        public override Expression ExpWith(Rational other)
        {
            return new Irrational(Math.Pow((double)@decimal, (double)other.@decimal));
        }

        public override Expression ExpWith(Irrational other)
        {
            return new Irrational(Math.Pow((double)@decimal, (double)other.@decimal));
        }

        #endregion

        #region GreaterThan
        public override Expression GreaterThan(Integer other)
        {
            return new Boolean(@decimal > other.@decimal);
        }

        public override Expression GreaterThan(Rational other)
        {
            return new Boolean(@decimal > other.@decimal);
        }

        public override Expression GreaterThan(Irrational other)
        {
            return new Boolean(@decimal > other.@decimal);
        }

        #endregion

        #region LesserThan
        public override Expression LesserThan(Integer other)
        {
            return new Boolean(@decimal < other.@decimal);
        }

        public override Expression LesserThan(Rational other)
        {
            return new Boolean(@decimal < other.@decimal);
        }

        public override Expression LesserThan(Irrational other)
        {
            return new Boolean(@decimal < other.@decimal);
        }

        #endregion

        #region GreaterThanEqualTo
        public override Expression GreaterThanOrEqualTo(Integer other)
        {
            return new Boolean(@decimal >= other.@decimal);
        }

        public override Expression GreaterThanOrEqualTo(Rational other)
        {
            return new Boolean(@decimal >= other.@decimal);
        }

        public override Expression GreaterThanOrEqualTo(Irrational other)
        {
            return new Boolean(@decimal >= other.@decimal);
        }

        #endregion

        #region LesserThanOrEqualTo
        public override Expression LesserThanOrEqualTo(Integer other)
        {
            return new Boolean(@decimal <= other.@decimal);
        }

        public override Expression LesserThanOrEqualTo(Rational other)
        {
            return new Boolean(@decimal <= other.@decimal);
        }

        public override Expression LesserThanOrEqualTo(Irrational other)
        {
            return new Boolean(@decimal <= other.@decimal);
        }

        #endregion

        #region ModuloWith
        public override Expression ModWith(Integer other)
        {
            return new Irrational(@decimal % other.@decimal);
        }

        public override Expression ModWith(Rational other)
        {
            return new Irrational(@decimal % other.@decimal);
        }

        public override Expression ModWith(Irrational other)
        {
            return new Irrational(@decimal % other.@decimal);
        }

        #endregion
    }
}

