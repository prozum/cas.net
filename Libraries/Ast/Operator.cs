using System;

namespace Ast
{
	public abstract class Operator : Expression
	{
		public string symbol;
		public int priority;
		public Expression left,right;

		public abstract Expression Evaluate(Expression a, Expression b);

		public override string ToString()
		{
			if (parent == null || priority >= parent.priority ) {

				return left.ToString () + symbol + right.ToString ();
			
			} else {

				return '(' + left.ToString () + symbol + right.ToString () + ')';

			}
		}
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

	public class Lesser : Operator
	{
		public Lesser()
		{
			symbol = "<";
			priority = 0;
		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Greater : Operator
	{
		public Greater()
		{
			symbol = ">";
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
			throw new NotImplementedException ();
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
