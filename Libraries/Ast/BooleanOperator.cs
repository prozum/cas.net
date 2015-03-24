using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast
{
    public class BooleanEqual : Operator
    {
        public BooleanEqual()
        {
            symbol = "==";
            priority = 0;
        }

        public override Expression Evaluate(Expression a, Expression b)
        {
            return new Boolean((new Greater().Evaluate(a, b) as Boolean).value == false && (new Lesser().Evaluate(a, b) as Boolean).value == false);
        }
    }

    public class Lesser : Operator
    {
        public Lesser()
        {
            symbol = "<";
            priority = 0;
        }

        public override Expression Evaluate(Expression a, Expression b)
        {
            return new Greater().Evaluate(b, a);
        }
    }

    public class LesserOrEqual : Operator
    {
        public LesserOrEqual()
        {
            symbol = "<=";
            priority = 0;
        }

        public override Expression Evaluate(Expression a, Expression b)
        {
            return new Boolean((new Lesser().Evaluate(a, b) as Boolean).value == true || (new BooleanEqual().Evaluate(a, b) as Boolean).value == true);
        }
    }

    public class Greater : Operator
    {
        public Greater()
        {
            symbol = ">";
            priority = 0;
        }

        public override Expression Evaluate(Expression a, Expression b)
        {
            if (a is Integer && b is Integer)
            {
                return new Boolean((a as Integer).value > (b as Integer).value);
            }

            if (a is Integer && b is Rational)
            {
                return new Greater().Evaluate(new Rational((a as Integer), new Integer(1)), b);
            }

            if (a is Rational && b is Integer)
            {
                return new Greater().Evaluate(a, new Rational((b as Integer), new Integer(1)));
            }


            if (a is Rational && b is Rational)
            {
                return new Boolean((a as Rational).numerator.value * (b as Rational).denominator.value > (b as Rational).numerator.value * (a as Rational).denominator.value);
            }

            if (a is Integer && b is Irrational)
            {
                return new Boolean((a as Integer).value > (b as Irrational).value);
            }

            if (a is Irrational && b is Integer)
            {
                return new Boolean((a as Irrational).value > (b as Integer).value);
            }

            if (a is Irrational && b is Irrational)
            {
                return new Boolean((a as Irrational).value > (b as Irrational).value);
            }

            if (a is Irrational && b is Rational)
            {
                return new Boolean((a as Irrational).value > (b as Rational).value.value);
            }

            if (a is Rational && b is Irrational)
            {
                return new Boolean((a as Rational).value.value > (b as Irrational).value);
            }

            return null;
        }
    }

    public class GreaterOrEqual : Operator
    {
        public GreaterOrEqual()
        {
            symbol = ">=";
            priority = 0;
        }

        public override Expression Evaluate(Expression a, Expression b)
        {
            return new Boolean((new Greater().Evaluate(a, b) as Boolean).value == true || (new BooleanEqual().Evaluate(a, b) as Boolean).value == true);
        }
    }

}
