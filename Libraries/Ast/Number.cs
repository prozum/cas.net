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

        public override bool CompareTo(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    if (value == (other as Integer).value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (other is Rational)
                {
                    if (value == (other as Rational).value.value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (other is Irrational)
                {
                    if (value == (other as Irrational).value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
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
            value = new Irrational((decimal)num.value / denom.value);
        }

        public override string ToString()
        {
            return value.ToString ();
        }

        public void Reduce(Integer num, Integer denom)
        {
            Gcd (num, denom);
        }

        public static Integer Gcd(Integer num, Integer denom)
        {
            throw new NotImplementedException ();
        }

        public override bool CompareTo(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    if (value.value == (other as Integer).value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (other is Rational)
                {
                    if (value.value == (other as Rational).value.value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (other is Irrational)
                {
                    if (value.value == (other as Irrational).value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
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

        public override bool CompareTo(Expression other)
        {
            if (other is Number)
            {
                if (other is Integer)
                {
                    if (value == (other as Integer).value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (other is Rational)
                {
                    if (value == (other as Rational).value.value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (other is Irrational)
                {
                    if (value == (other as Irrational).value)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
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

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (real == (other as Complex).real && real == (imag as Complex).imag)
                {
                    res = true;
                }
            }

            return res;
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

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (value == (other as Boolean).value)
                {
                    res = true;
                }
            }

            return res;
        }
    }


}

