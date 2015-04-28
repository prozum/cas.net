using System;

namespace Ast
{
    public class Complex : Number 
    {
        public Number real;
        public Number imag;

        public Complex(Number real, Number imag)
        {
            this.real = real;
            this.imag = imag;
        }

        public override string ToString()
        {
            return real.ToString () + '+' + imag.ToString() + 'i';
        }

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (!real.CompareTo((other as Complex).real) || !imag.CompareTo((imag as Complex).imag))
                {
                    res = false;
                }
            }

            return res;
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

