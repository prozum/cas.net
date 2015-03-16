using System;

namespace Ast
{
	public abstract class Operator
	{
		public abstract Expression Evaluate(Expression a, Expression b);
	}

	public class Equals : Operator
	{
		public Equals()
		{

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

		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Sub : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Mul : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Div : Operator
	{

		/* fix errors */
		public override Expression Evaluate (Expression a, Expression b)
		{
			if (a is Integer && b is Integer) {
				return new Rational(a, b);
			}

			if (a is Integer && b is Rational) {
				return new Div().Evaluate(new Rational(((Integer)a).value, 1), b);
			}

			if (a is Rational && b is Integer) {
				return new Div().Evaluate(new Rational(b, ((Integer)b).value, 1));
			}

			if (a is Rational && b is Rational) {
				return new Mul(new Rational(((Rational)a).numerator.value, ((Rational)a).denominator.value), b);
			}

			if (a is Integer && b is Irrational)
			{
				return new Irrational(((Integer)a).value / ((Irrational)b).value);
			}

			if (a is Irrational && b is Integer)
			{
				return new Irrational(((Irrational)a).value / ((Integer)b).value);
			}

			if (a is Irrational && b is Irrational)
			{
				return new Irrational(((Irrational)a).value / ((Irrational)b).value);
			}

			if (a is Irrational && b is Rational)
			{
				return new Irrational(((Irrational)a).value / ((Rational)b).value);
			}

			if (a is Rational && b is Irrational)
			{
				return new Irrational(((Rational)a).value / ((Irrational)b).value);
			}
		}
	}

	public class Exp : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class LesserThan : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class GreaterThan : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}
}

