using System;

namespace Ast
{
	public abstract class Number : Expression 
	{
	}

	public class Integer : Number 
	{


		public int value;

		public Integer(int val)
		{
			value = val;
		}
	}

	public class Rational : Number 
	{
		public Integer numerator;
		public Integer denominator;
		public Expression value;

		public Rational(Integer num, Integer denom)
		{
			numerator = num;
			denominator = denom;
			value = new Div (numerator, denominator).Evaluate(numerator, denominator);
		}

		public void Reduce(Integer num, Integer denom)
		{
			Gcd (num, denom);
		}

		public static Integer Gcd(Integer num, Integer denom)
		{
			return new Integer (null);
		}
	}

	public class Irrational : Number 
	{
		public decimal value;

		public Irrational(decimal val)
		{
			value = val;
		}
	}

	public class Complex : Number 
	{
		public Number real;
		public Number imag;
	}
}

