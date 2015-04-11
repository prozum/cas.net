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
            priority = 10;
        }

        public override Expression Evaluate()
        {
            return new Boolean(left.CompareTo(right));
        }
    }

    public class Lesser : Operator
    {
        public Lesser() : this(null, null) { }
        public Lesser(Expression left, Expression right) : base(left, right)
        {
            symbol = "<";
            priority = 10;
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
            priority = 10;
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
            priority = 10;
        }

        public override Expression Evaluate()
        {
            Expression evaluatedLeft, evaluatedRight;

            if (!((evaluatedLeft = left.Evaluate()) is Error || (evaluatedRight = right.Evaluate()) is Error))
            {
                if (evaluatedLeft is Integer && evaluatedRight is Integer)
                {
                    return new Boolean((evaluatedLeft as Integer).value > (evaluatedRight as Integer).value);
                }

                if (evaluatedLeft is Integer && evaluatedRight is Rational)
                {
                    return new Greater(new Rational((evaluatedLeft as Integer), new Integer(1)), evaluatedRight).Evaluate();
                }

                if (evaluatedLeft is Rational && evaluatedRight is Integer)
                {
                    return new Greater(evaluatedLeft, new Rational((evaluatedRight as Integer), new Integer(1))).Evaluate();
                }

                if (evaluatedLeft is Rational && evaluatedRight is Rational)
                {
                    return new Boolean((evaluatedLeft as Rational).numerator.value * (evaluatedRight as Rational).denominator.value > (evaluatedRight as Rational).numerator.value * (evaluatedLeft as Rational).denominator.value);
                }

                if (evaluatedLeft is Integer && evaluatedRight is Irrational)
                {
                    return new Boolean((evaluatedLeft as Integer).value > (evaluatedRight as Irrational).value);
                }

                if (evaluatedLeft is Irrational && evaluatedRight is Integer)
                {
                    return new Boolean((evaluatedLeft as Irrational).value > (evaluatedRight as Integer).value);
                }

                if (evaluatedLeft is Irrational && evaluatedRight is Irrational)
                {
                    return new Boolean((evaluatedLeft as Irrational).value > (evaluatedRight as Irrational).value);
                }

                if (evaluatedLeft is Irrational && evaluatedRight is Rational)
                {
                    return new Boolean((evaluatedLeft as Irrational).value > (evaluatedRight as Rational).value.value);
                }

                if (evaluatedLeft is Rational && evaluatedRight is Irrational)
                {
                    return new Boolean((evaluatedLeft as Rational).value.value > (evaluatedRight as Irrational).value);
                }

                return new Boolean(false);
            }
            else
            {
                return new Boolean(false);
            }
        }
    }

    public class GreaterOrEqual : Operator
    {
        public GreaterOrEqual() : this(null, null) { }
        public GreaterOrEqual(Expression left, Expression right) : base(left, right)
        {
            symbol = ">=";
            priority = 10;
        }

        public override Expression Evaluate()
        {
            return new Boolean((new Greater(left, right).Evaluate() as Boolean).value == true || (new BooleanEqual(left, right).Evaluate() as Boolean).value == true);
        }
    }

}
