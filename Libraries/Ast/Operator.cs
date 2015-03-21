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
			if (parent != null && priority >= parent.priority) {

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
			throw new NotImplementedException ();
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
			throw new NotImplementedException ();
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
				return new Rational((Integer)a, (Integer)b);
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

			throw new NotImplementedException ();

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
