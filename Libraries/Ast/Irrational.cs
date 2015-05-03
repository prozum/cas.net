using System;

namespace Ast
{
    public class Irrational : Real, INegative 
    {
        public decimal @decimal;

        public Irrational(double value) : this ((decimal)value) {}
        public Irrational(decimal value)
        {
            this.@decimal = value;
        }

        public override decimal Value
        {
            get
            {
                return @decimal;
            }
        }

        public override Expression Clone()
        {
            return new Irrational(@decimal);
        }

        public Expression ToNegative()
        {
            return new Irrational(@decimal *= -1);
        }

        public override Expression Minus()
        {
            return ToNegative();
        }
    }
}

