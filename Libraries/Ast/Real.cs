using System;

namespace Ast
{
    public abstract class Real : Number
    {
        public abstract Decimal Value
        {
            get;
        }
            
        public static implicit operator Decimal(Real r)
        {
            return r.Value;
        }

        public static implicit operator double(Real r)
        {
            return (Double)r.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool CompareTo(Expression other)
        {
            Expression evalOther = other.Evaluate();

            if (evalOther is Real)
            {
                return Value == (evalOther as Real).Value;
            }

            return false;
        }

        public bool IsNegative()
        {
            return Value < 0;
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return new Irrational(Value + other.Value);
        }

        public override Expression AddWith(Rational other)
        {
            return new Irrational(Value + other.Value);
        }

        public override Expression AddWith(Irrational other)
        {
            return new Irrational(Value + other.Value);
        }

        public override Expression AddWith(Text other)
        {
            return new Text(Value + other.Value);
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return new Irrational(Value - other.Value);
        }

        public override Expression SubWith(Rational other)
        {
            return new Irrational(Value - other.Value);
        }

        public override Expression SubWith(Irrational other)
        {
            return new Irrational(Value - other.Value);
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return new Irrational(Value * other.Value);
        }

        public override Expression MulWith(Rational other)
        {
            return new Irrational(Value * other.Value);
        }

        public override Expression MulWith(Irrational other)
        {
            return new Irrational(Value * other.Value);
        }

        #endregion

        #region DivWith
        public override Expression DivWith(Integer other)
        {
            return new Irrational(Value / other.Value);
        }

        public override Expression DivWith(Rational other)
        {
            return new Irrational(Value / other.Value);
        }

        public override Expression DivWith(Irrational other)
        {
            return new Irrational(Value / other.Value);
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return new Irrational(Math.Pow((double)Value, (double)other.Value));
        }

        public override Expression ExpWith(Rational other)
        {
            return new Irrational(Math.Pow((double)Value, (double)other.Value));
        }

        public override Expression ExpWith(Irrational other)
        {
            return new Irrational(Math.Pow((double)Value, (double)other.Value));
        }

        #endregion

        #region GreaterThan
        public override Expression GreaterThan(Integer other)
        {
            return new Boolean(Value > other.Value);
        }

        public override Expression GreaterThan(Rational other)
        {
            return new Boolean(Value > other.Value);
        }

        public override Expression GreaterThan(Irrational other)
        {
            return new Boolean(Value > other.Value);
        }

        #endregion

        #region LesserThan
        public override Expression LesserThan(Integer other)
        {
            return new Boolean(Value < other.Value);
        }

        public override Expression LesserThan(Rational other)
        {
            return new Boolean(Value < other.Value);
        }

        public override Expression LesserThan(Irrational other)
        {
            return new Boolean(Value < other.Value);
        }

        #endregion

        #region GreaterThanEqualTo
        public override Expression GreaterThanOrEqualTo(Integer other)
        {
            return new Boolean(Value >= other.Value);
        }

        public override Expression GreaterThanOrEqualTo(Rational other)
        {
            return new Boolean(Value >= other.Value);
        }

        public override Expression GreaterThanOrEqualTo(Irrational other)
        {
            return new Boolean(Value >= other.Value);
        }

        #endregion

        #region LesserThanOrEqualTo
        public override Expression LesserThanOrEqualTo(Integer other)
        {
            return new Boolean(Value <= other.Value);
        }

        public override Expression LesserThanOrEqualTo(Rational other)
        {
            return new Boolean(Value <= other.Value);
        }

        public override Expression LesserThanOrEqualTo(Irrational other)
        {
            return new Boolean(Value <= other.Value);
        }

        #endregion

        #region ModuloWith
        public override Expression ModuloWith(Integer other)
        {
            return new Irrational(Value % other.Value);
        }

        public override Expression ModuloWith(Rational other)
        {
            return new Irrational(Value % other.Value);
        }

        public override Expression ModuloWith(Irrational other)
        {
            return new Irrational(Value % other.Value);
        }

        #endregion
    }
}

