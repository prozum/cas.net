using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
	public abstract class Expression 
	{
		public Operator parent;
		//public abstract Expression Evaluate (Expression a, Expression b);
		//public abstract string ToString ();
		//public abstract bool Contains (Expression a);

        #region Add
        public static Integer operator +(Integer a, Integer b)
        {
            return new Integer(a.value + b.value);
        }

        public static Rational operator +(Integer a, Rational b)
        {
            return new Rational(a, new Integer(1)) + b;
        }

        public static Rational operator +(Rational a, Integer b)
        {
            return a + new Rational(b, new Integer(1));
        }

        public static Rational operator +(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public static Irrational operator +(Integer a, Irrational b)
        {
            return new Irrational(a.value + b.value);
        }

        public static Irrational operator +(Irrational a, Integer b)
        {
            return new Irrational(a.value + b.value);
        }

        public static Irrational operator +(Irrational a, Rational b)
        {
            return new Irrational(a.value + b.value.value);
        }

        public static Irrational operator +(Rational a, Irrational b)
        {
            return new Irrational(a.value.value + b.value);
        }

        public static Irrational operator +(Irrational a, Irrational b)
        {
            return new Irrational(a.value + b.value);
        }
        #endregion

        #region Sub
        public static Integer operator -(Integer a, Integer b)
        {
            return new Integer(a.value - b.value);
        }

        public static Rational operator -(Integer a, Rational b)
        {
            return new Rational(a, new Integer(1)) - b;
        }

        public static Rational operator -(Rational a, Integer b)
        {
            return a + new Rational(b, new Integer(1));
        }

        public static Rational operator -(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.denominator - b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public static Irrational operator -(Integer a, Irrational b)
        {
            return new Irrational(a.value - b.value);
        }

        public static Irrational operator -(Irrational a, Integer b)
        {
            return new Irrational(a.value - b.value);
        }

        public static Irrational operator -(Irrational a, Rational b)
        {
            return new Irrational(a.value - b.value.value);
        }

        public static Irrational operator -(Rational a, Irrational b)
        {
            return new Irrational(a.value.value - b.value);
        }

        public static Irrational operator -(Irrational a, Irrational b)
        {
            return new Irrational(a.value - b.value);
        }
        #endregion

        #region Mul
        public static Integer operator *(Integer a, Integer b)
        {
            return new Integer(a.value * b.value);
        }

        public static Rational operator *(Integer a, Rational b)
        {
            return new Rational(a, new Integer(1)) * b;
        }

        public static Rational operator *(Rational a, Integer b)
        {
            return a * new Rational(b, new Integer(1));
        }

        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Irrational operator *(Integer a, Irrational b)
        {
            return new Irrational(a.value * b.value);
        }

        public static Irrational operator *(Irrational a, Integer b)
        {
            return new Irrational(a.value * b.value);
        }

        public static Irrational operator *(Irrational a, Rational b)
        {
            return new Irrational(a.value * b.value.value);
        }

        public static Irrational operator *(Rational a, Irrational b)
        {
            return new Irrational(a.value.value * b.value);
        }

        public static Irrational operator *(Irrational a, Irrational b)
        {
            return new Irrational(a.value * b.value);
        }
        #endregion

        #region Div
        public static Rational operator /(Integer a, Integer b)
        {
            return new Rational(a, b);
        }

        public static Rational operator /(Integer a, Rational b)
        {
            return new Rational(a , new Integer(1)) / b;
        }

        public static Rational operator /(Rational a, Integer b)
        {
            return a / new Rational(b, new Integer(1));
        }

        public static Rational operator /(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public static Irrational operator /(Integer a, Irrational b)
        {
            return new Irrational(a.value / b.value);
        }

        public static Irrational operator /(Irrational a, Integer b)
        {
            return new Irrational(a.value / b.value);
        }

        public static Irrational operator /(Irrational a, Rational b)
        {
            return new Irrational(a.value / b.value.value);
        }

        public static Irrational operator /(Rational a, Irrational b)
        {
            return new Irrational(a.value.value / b.value);
        }

        public static Irrational operator /(Irrational a, Irrational b)
        {
            return new Irrational(a.value / b.value);
        }
        #endregion

	}
}