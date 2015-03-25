using System;

namespace Ast
{
	public abstract class Operator : Expression
	{
		public string symbol;
		public int priority;
		public Expression left,right;

		public abstract Expression Evaluate(Expression a, Expression b);

        //public override string ToString()
        //{
        //    if (parent == null || priority >= parent.priority ) {

        //        return left.ToString () + symbol + right.ToString ();
			
        //    } else {

        //        return '(' + left.ToString () + symbol + right.ToString () + ')';

        //    }
        //}

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
            return new Rational(new Integer(a.numerator * b.denominator + b.numerator * a.denominator) , new Integer(a.denominator * b.denominator));
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
        public static Expression operator -(Integer a, Integer b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Integer a, Rational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Rational a, Integer b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Rational a, Rational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Integer a, Irrational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Irrational a, Integer b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Irrational a, Rational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Rational a, Irrational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator -(Irrational a, Irrational b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Mul
        public static Integer operator *(Integer a, Integer b)
        {
            return new Integer(a.value * b.value);
        }

        public static Rational operator *(Integer a, Rational b)
        {
            return new Rational(new Rational(a, new Integer(1)) * b);
        }

        public static Rational operator *(Rational a, Integer b)
        {
            return new Rational(a * new Rational(b, new Integer(1)));
        }

        public static Expression operator *(Rational a, Rational b)
        {
            return new Rational(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Expression operator *(Integer a, Irrational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator *(Irrational a, Integer b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator *(Irrational a, Rational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator *(Rational a, Irrational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator *(Irrational a, Irrational b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Div
        public static Expression operator /(Integer a, Integer b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Integer a, Rational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Rational a, Integer b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Rational a, Rational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Integer a, Irrational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Irrational a, Integer b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Irrational a, Rational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Rational a, Irrational b)
        {
            throw new NotImplementedException();
        }

        public static Expression operator /(Irrational a, Irrational b)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

	public class Equal : Operator
	{
		public Equal()
		{
			symbol = "=";
			priority = 0;
		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}

	}

	public class Add : Operator
	{
		public Add()
		{
			symbol = "+";
			priority = 10;
		}

		public override Expression Evaluate (Expression a, Expression b)
		{
            if (a is Integer && b is Integer)
            {
                return new Integer((a as Integer).value + (b as Integer).value);
            }

            if (a is Integer && b is Rational)
            {
                return new Sub().Evaluate(new Rational((a as Integer), new Integer(1)), b);
            }

            if (a is Rational && b is Integer)
            {
                return new Sub().Evaluate(a, new Rational((b as Integer), new Integer(1)));
            }


            if (a is Rational && b is Rational)
            {
                (a as Rational).numerator.value *= (b as Rational).denominator.value;
                (b as Rational).numerator.value *= (a as Rational).denominator.value;
                return new Rational((new Add().Evaluate((a as Rational).numerator, (b as Rational).numerator) as Integer),
                                    (new Mul().Evaluate((b as Rational).denominator, (a as Rational).denominator)) as Integer);
            }

            if (a is Integer && b is Irrational)
            {
                return new Irrational((a as Integer).value + (b as Irrational).value);
            }

            if (a is Irrational && b is Integer)
            {
                return new Irrational((a as Irrational).value + (b as Integer).value);
            }

            if (a is Irrational && b is Irrational)
            {
                return new Irrational((a as Irrational).value + (b as Irrational).value);
            }

            if (a is Irrational && b is Rational)
            {
                return new Irrational((a as Irrational).value + (b as Rational).value.value);
            }

            if (a is Rational && b is Irrational)
            {
                return new Irrational((a as Rational).value.value + (b as Irrational).value);
            }

            return null;
		}
	}

	public class Sub : Operator
	{
		public Sub()
		{
			symbol = "-";
			priority = 10;
		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			if (a is Integer && b is Integer) {
				return new Integer((a as Integer).value - (b as Integer).value);
			}

			if (a is Integer && b is Rational) {
				return new Sub().Evaluate (new Rational((a as Integer), new Integer(1)), b);
			}

			if (a is Rational && b is Integer) {
				return new Sub().Evaluate(a, new Rational((b as Integer), new Integer(1)));
			}


			if (a is Rational && b is Rational) {
				(a as Rational).numerator.value *= (b as Rational).denominator.value;
				(b as Rational).numerator.value *= (a as Rational).denominator.value;
				return new Rational((new Sub().Evaluate((a as Rational).numerator, (b as Rational).numerator) as Integer),
									(new Mul().Evaluate((b as Rational).denominator, (a as Rational).denominator)) as Integer);
			}

			if (a is Integer && b is Irrational) {
				return new Irrational((a as Integer).value - (b as Irrational).value);
			}

			if (a is Irrational && b is Integer) {
				return new Irrational((a as Irrational).value - (b as Integer).value);
			}

			if (a is Irrational && b is Irrational) {
				return new Irrational((a as Irrational).value - (b as Irrational).value);
			}

			if (a is Irrational && b is Rational) {
				return new Irrational((a as Irrational).value - (b as Rational).value.value);
			}

			if (a is Rational && b is Irrational) {
				return new Irrational((a as Rational).value.value - (b as Irrational).value);
			}

			return null;
		}
	}

	public class Mul : Operator
	{
		public Mul()
		{
			symbol = "*";
			priority = 20;
		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			if (a is Integer && b is Integer) {
				return new Integer((a as Integer).value * (b as Integer).value);
			}

			if (a is Integer && b is Rational) {
				return new Mul().Evaluate (new Rational ((a as Integer), new Integer(1)), b);
			}

			if (a is Rational && b is Integer) {
				return new Mul().Evaluate(a, new Rational((b as Integer), new Integer(1)));
			}

			if (a is Rational && b is Rational) {
				return new Rational(((Integer)new Mul().Evaluate((a as Rational).numerator, (b as Rational).numerator)),
									((Integer)new Mul().Evaluate((a as Rational).denominator, (b as Rational).denominator)));
			}

			if (a is Integer && b is Irrational) {
				return new Irrational((a as Integer).value * (b as Irrational).value);
			}

			if (a is Irrational && b is Integer) {
				return new Irrational((a as Irrational).value * (b as Integer).value);
			}

			if (a is Irrational && b is Irrational) {
				return new Irrational((a as Irrational).value * (b as Irrational).value);
			}

			if (a is Irrational && b is Rational) {
				return new Irrational((a as Irrational).value * (b as Rational).value.value);
			}

			if (a is Rational && b is Irrational) {
				return new Irrational((a as Rational).value.value * (b as Irrational).value);
			}

			return null;
		}
	}

	public class Div : Operator
	{
		public Div()
		{
			symbol = "/";
			priority = 20;
		}

		/* fix errors */
		public override Expression Evaluate (Expression a, Expression b)
		{
			if (a is Integer && b is Integer) {
				return new Rational((a as Integer), (b as Integer));
			}

			if (a is Integer && b is Rational) {
				return new Div().Evaluate(new Rational((a as Integer), new Integer(1)), b);
			}

			if (a is Rational && b is Integer) {
				return new Div().Evaluate(a, new Rational((b as Integer), new Integer(1)));
			}

			if (a is Rational && b is Rational) {
				return new Mul().Evaluate(new Rational((a as Rational).denominator, (a as Rational).numerator), b);
			}

			if (a is Integer && b is Irrational) {
				return new Irrational((a as Integer).value / (b as Irrational).value);
			}

			if (a is Irrational && b is Integer) {
				return new Irrational((a as Irrational).value / (b as Integer).value);
			}

			if (a is Irrational && b is Irrational) {
				return new Irrational((a as Irrational).value / (b as Irrational).value);
			}

			if (a is Irrational && b is Rational) {
				return new Irrational((a as Irrational).value / (b as Rational).value.value);
			}

			if (a is Rational && b is Irrational) {
				return new Irrational((a as Rational).value.value / (b as Irrational).value);
			}

			return null;
		}
	}

	public class Exp : Operator
	{
		public Exp()
		{
			symbol = "^";
			priority = 30;
		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}
}
