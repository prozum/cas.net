using System;

namespace Ast
{
	public abstract class Operator : Expression
	{
		public string symbol;
		public int priority;
		public Expression left,right;

		public abstract Expression Evaluate();

        public Operator(Expression left, Expression right)
        {
            this.left = left;
            this.right = right;
        }

        //public override string ToString()
        //{
        //    if (parent == null || priority >= parent.priority ) {

        //        return left.ToString () + symbol + right.ToString ();
			
        //    } else {

        //        return '(' + left.ToString () + symbol + right.ToString () + ')';

        //    }
        //}

    }

	public class Equal : Operator
	{
        public Equal() : this(null, null) { }
        public Equal(Expression left, Expression right) : base(left, right)
        {
            symbol = "=";
            priority = 0;
        }

		public override Expression Evaluate ()
		{
			throw new NotImplementedException ();
		}

	}

	public class Add : Operator
	{
        public Add() : this(null, null) { }
        public Add(Expression left, Expression right) : base(left, right)
		{
			symbol = "+";
			priority = 10;
		}

		public override Expression Evaluate ()
		{
            if (left is Integer && right is Integer)
            {
                return new Integer((left as Integer).value + (right as Integer).value);
            }

            if (left is Integer && right is Rational)
            {
                return new Sub(new Rational((left as Integer), new Integer(1)), right).Evaluate();
            }

            if (left is Rational && right is Integer)
            {
                return new Sub(left, new Rational((right as Integer), new Integer(1))).Evaluate();
            }


            if (left is Rational && right is Rational)
            {
                (left as Rational).numerator.value *= (right as Rational).denominator.value;
                (right as Rational).numerator.value *= (left as Rational).denominator.value;
                return new Rational(new Add((left as Rational).numerator, (right as Rational).numerator).Evaluate() as Integer,
                                    new Mul((right as Rational).denominator, (left as Rational).denominator).Evaluate() as Integer);
            }

            if (left is Integer && right is Irrational)
            {
                return new Irrational((left as Integer).value + (right as Irrational).value);
            }

            if (left is Irrational && right is Integer)
            {
                return new Irrational((left as Irrational).value + (right as Integer).value);
            }

            if (left is Irrational && right is Irrational)
            {
                return new Irrational((left as Irrational).value + (right as Irrational).value);
            }

            if (left is Irrational && right is Rational)
            {
                return new Irrational((left as Irrational).value + (right as Rational).value.value);
            }

            if (left is Rational && right is Irrational)
            {
                return new Irrational((left as Rational).value.value + (right as Irrational).value);
            }

            return null;
		}
	}

	public class Sub : Operator
	{
        public Sub() : this(null, null) { }
        public Sub(Expression left, Expression right) : base(left, right)
		{
			symbol = "-";
			priority = 10;
		}

		public override Expression Evaluate ()
		{
			if (left is Integer && right is Integer) {
				return new Integer((left as Integer).value - (right as Integer).value);
			}

			if (left is Integer && right is Rational) {
                return new Sub(new Rational((left as Integer), new Integer(1)), right).Evaluate();
			}

			if (left is Rational && right is Integer) {
                return new Sub(left, new Rational((right as Integer), new Integer(1))).Evaluate();
			}


			if (left is Rational && right is Rational) {
				(left as Rational).numerator.value *= (right as Rational).denominator.value;
				(right as Rational).numerator.value *= (left as Rational).denominator.value;
                return new Rational(new Sub((left as Rational).numerator, (right as Rational).numerator).Evaluate() as Integer,
                                    new Mul((right as Rational).denominator, (left as Rational).denominator).Evaluate() as Integer);
			}

			if (left is Integer && right is Irrational) {
				return new Irrational((left as Integer).value - (right as Irrational).value);
			}

			if (left is Irrational && right is Integer) {
				return new Irrational((left as Irrational).value - (right as Integer).value);
			}

			if (left is Irrational && right is Irrational) {
				return new Irrational((left as Irrational).value - (right as Irrational).value);
			}

			if (left is Irrational && right is Rational) {
				return new Irrational((left as Irrational).value - (right as Rational).value.value);
			}

			if (left is Rational && right is Irrational) {
				return new Irrational((left as Rational).value.value - (right as Irrational).value);
			}

			return null;
		}
	}

	public class Mul : Operator
	{
        public Mul() : this(null, null) { }
        public Mul(Expression left, Expression right) : base(left, right)
		{
			symbol = "*";
			priority = 20;
		}

		public override Expression Evaluate ()
		{
			if (left is Integer && right is Integer) {
				return new Integer((left as Integer).value * (right as Integer).value);
			}

			if (left is Integer && right is Rational) {
                return new Mul(new Rational((left as Integer), new Integer(1)), right).Evaluate();
			}

			if (left is Rational && right is Integer) {
                return new Mul(left, new Rational((right as Integer), new Integer(1))).Evaluate();
			}

			if (left is Rational && right is Rational) {
                return new Rational(new Mul((left as Rational).numerator, (right as Rational).numerator).Evaluate() as Integer,
                                   new Mul((left as Rational).denominator, (right as Rational).denominator).Evaluate() as Integer);
			}

			if (left is Integer && right is Irrational) {
				return new Irrational((left as Integer).value * (right as Irrational).value);
			}

			if (left is Irrational && right is Integer) {
				return new Irrational((left as Irrational).value * (right as Integer).value);
			}

			if (left is Irrational && right is Irrational) {
				return new Irrational((left as Irrational).value * (right as Irrational).value);
			}

			if (left is Irrational && right is Rational) {
				return new Irrational((left as Irrational).value * (right as Rational).value.value);
			}

			if (left is Rational && right is Irrational) {
				return new Irrational((left as Rational).value.value * (right as Irrational).value);
			}

			return null;
		}
	}

	public class Div : Operator
	{
        public Div() : this(null, null) { }
        public Div(Expression left, Expression right) : base(left, right)
		{
			symbol = "/";
			priority = 20;
		}

		/* fix errors */
		public override Expression Evaluate ()
		{
			if (left is Integer && right is Integer) {
				return new Rational((left as Integer), (right as Integer));
			}

			if (left is Integer && right is Rational) {
                return new Div(new Rational((left as Integer), new Integer(1)), right).Evaluate();
			}

			if (left is Rational && right is Integer) {
                return new Div(left, new Rational((right as Integer), new Integer(1))).Evaluate();
			}

			if (left is Rational && right is Rational) {
                return new Mul(new Rational((left as Rational).denominator, (left as Rational).numerator), right).Evaluate();
			}

			if (left is Integer && right is Irrational) {
				return new Irrational((left as Integer).value / (right as Irrational).value);
			}

			if (left is Irrational && right is Integer) {
				return new Irrational((left as Irrational).value / (right as Integer).value);
			}

			if (left is Irrational && right is Irrational) {
				return new Irrational((left as Irrational).value / (right as Irrational).value);
			}

			if (left is Irrational && right is Rational) {
				return new Irrational((left as Irrational).value / (right as Rational).value.value);
			}

			if (left is Rational && right is Irrational) {
				return new Irrational((left as Rational).value.value / (right as Irrational).value);
			}

			return null;
		}
	}

	public class Exp : Operator
	{
        public Exp() : this(null, null) { }
        public Exp(Expression left, Expression right) : base(left, right)
		{
			symbol = "^";
			priority = 30;
		}

        public override Expression Evaluate()
        {
            throw new NotImplementedException();
        }
	}
}
