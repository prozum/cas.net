using System;

namespace Ast
{
    public class Irrational : Real, INegative 
    {
        public decimal _decimal;

        public Irrational(double value) : this ((decimal)value) {}
        public Irrational(decimal value)
        {
            _decimal = value;
        }

        public override decimal @decimal
        {
            get
            {
                return _decimal;
            }
        }

        public override Expression Clone()
        {
            return new Irrational(@decimal);
        }

        public Expression ToNegative()
        {
            return new Irrational(-@decimal);
        }

        public override Expression Minus()
        {
            return ToNegative();
        }
    }
}

