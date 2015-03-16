using System;

namespace Ast
{
	public abstract class Number : Expression 
	{
	}

	public class Integer : Number 
	{
		public int value;
	}

	public class Rational : Number 
	{
		public Integer numerator;
		public Integer denominator;
		public Integer gcd;

		public void Reduce(Integer num, Integer denom)
		{
			Gcd (num, denom);
		}

		public static Integer Gcd(Integer num, Integer denom)
		{
			return new Integer ();
		}
	}

	public class Irrational : Number 
	{
		public decimal value;
	}

	public class Complex : Number 
	{
		public Number real;
		public Number imag;
	}
}

