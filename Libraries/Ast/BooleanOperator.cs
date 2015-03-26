using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast
{
    public class BooleanEqual : Operator
    {
        public BooleanEqual() : this(null, null) { }
        public BooleanEqual(Expression left, Expression right) : base(left, right)
        {
            symbol = "==";
            priority = 0;
        }

        public override Expression Evaluate()
        {
            return new Boolean((new Greater(left, right).Evaluate() as Boolean).value == false && (new Lesser(left, right).Evaluate() as Boolean).value == false);
        }
    }

    public class Lesser : Operator
    {
        public Lesser() : this(null, null) { }
        public Lesser(Expression left, Expression right) : base(left, right)
        {
            symbol = "<";
            priority = 0;
        }

        public override Expression Evaluate()
        {
            return new Greater(right, left).Evaluate();
        }
    }

    public class LesserOrEqual : Operator
    {
        public LesserOrEqual() : this(null, null) { }
        public LesserOrEqual(Expression left, Expression right) : base(left, right)
        {
            symbol = "<=";
            priority = 0;
        }

        public override Expression Evaluate()
        {
            return new Boolean((new Lesser(left, right).Evaluate() as Boolean).value == true || (new BooleanEqual(left, right).Evaluate() as Boolean).value == true);
        }
    }

    public class Greater : Operator
    {
        public Greater() : this(null, null) { }
        public Greater(Expression left, Expression right) : base(left, right)
        {
            symbol = ">";
            priority = 0;
        }

        public override Expression Evaluate()
        {
            if (left is Integer && right is Integer)
            {
                return new Boolean((left as Integer).value > (right as Integer).value);
            }

            if (left is Integer && right is Rational)
            {
                return new Greater(new Rational((left as Integer), new Integer(1)), right).Evaluate();
            }

            if (left is Rational && right is Integer)
            {
                return new Greater(left, new Rational((right as Integer), new Integer(1))).Evaluate();
            }

            if (left is Rational && right is Rational)
            {
                return new Boolean((left as Rational).numerator.value * (right as Rational).denominator.value > (right as Rational).numerator.value * (left as Rational).denominator.value);
            }

            if (left is Integer && right is Irrational)
            {
                return new Boolean((left as Integer).value > (right as Irrational).value);
            }

            if (left is Irrational && right is Integer)
            {
                return new Boolean((left as Irrational).value > (right as Integer).value);
            }

            if (left is Irrational && right is Irrational)
            {
                return new Boolean((left as Irrational).value > (right as Irrational).value);
            }

            if (left is Irrational && right is Rational)
            {
                return new Boolean((left as Irrational).value > (right as Rational).value.value);
            }

            if (left is Rational && right is Irrational)
            {
                return new Boolean((left as Rational).value.value > (right as Irrational).value);
            }

            return base.Evaluate();
        }
    }

    public class GreaterOrEqual : Operator
    {
        public GreaterOrEqual() : this(null, null) { }
        public GreaterOrEqual(Expression left, Expression right) : base(left, right)
        {
            symbol = ">=";
            priority = 0;
        }

        public override Expression Evaluate()
        {
            return new Boolean((new Greater(left, right).Evaluate() as Boolean).value == true || (new BooleanEqual(left, right).Evaluate() as Boolean).value == true);
        }
    }

}
