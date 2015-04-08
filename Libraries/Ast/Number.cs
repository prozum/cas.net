using System;

namespace Ast
{
    public abstract class Number : Expression 
    {

        public override Expression Evaluate()
        {
            return this;
        }
    }

    public class Integer : Number
    {
        public int value;

        public Integer(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString ();
        }
    }

    public class Rational : Number 
    {
        public Integer numerator;
        public Integer denominator;
        public Irrational value
        {
            get
            {
                return new Irrational(numerator.value / denominator.value);
            }
        }

        public Rational(Integer num, Integer denom)
        {
            numerator = num;
            denominator = denom;
            //value = new Irrational(num.value / denom.value);
        }

        public override string ToString()
        {
            return numerator.ToString () + "/" + denominator.ToString ();
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

        public Irrational(decimal value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString ();
        }
    }

    public class Complex : Number 
    {
        public Number real;
        public Number imag;

        public override string ToString()
        {
            return real.ToString () + '+' + imag.ToString() + 'i';
        }
    }

    public class Boolean : Number
    {
        public bool value;

        public Boolean(bool value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}

