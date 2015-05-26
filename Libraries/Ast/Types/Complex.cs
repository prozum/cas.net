namespace Ast
{
    public class Complex : Number
    {
        public Real real;
        public Real imag;

        public Complex(Real real, Real imag)
        {
            this.real = real;
            this.imag = imag;
        }

        public override string ToString()
        {
            if (imag.IsNegative())
                return "(" + real + imag.ToString() + "i)";
            else
                return "(" + real.ToString () + '+' + imag.ToString() + "i)";
        }

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (real.CompareTo((other as Complex).real) || imag.CompareTo((other as Complex).imag))
                {
                    res = false;
                }
            }

            return res;
        }

        public override Expression Clone()
        {
            return new Complex(real.Clone() as Real, imag.Clone() as Real);
        }

        public override Expression Minus()
        {
            return new Complex(real.ToNegative() as Real, imag.ToNegative() as Real);
        }

        public override Expression AddWith(Complex other)
        {
            return new Complex(real + other.real as Real, imag + other.imag as Real);
        }

        public override Expression SubWith(Complex other)
        {
            return new Complex(real - other.real as Real, imag - other.imag as Real);
        }
    }
}

