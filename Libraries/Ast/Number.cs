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
		}
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
			value = new Irrational(num.value / denom.value);
		}

		public void Reduce(Integer num, Integer denom)
		{
			Gcd (num, denom);
		}

		public static Integer Gcd(Integer num, Integer denom)
		{
			throw new NotImplementedException ();
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

