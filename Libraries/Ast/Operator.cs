using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class Operator : Expression
    {
        public string symbol;
        public int priority;
        public Expression left,right;

        public Operator(Expression left, Expression right)
        {
            this.left = left;
            this.right = right;
        }

        public override Expression Evaluate()
        {
            return new Error(this, "Cannot evaluate operator expression!");
        }

        public override string ToString()
        {
            if (parent == null || priority >= parent.priority)
            {
                return left.ToString () + symbol + right.ToString ();
            } 
            else 
            {
                return '(' + left.ToString () + symbol + right.ToString () + ')';
            }
        }

        public override void SetFunctionCall(UserDefinedFunction functionCall)
        {
            left.SetFunctionCall(functionCall);
            right.SetFunctionCall(functionCall);
            base.SetFunctionCall(functionCall);
        }

        public override bool CompareTo(Expression other)
        {
            Expression thisSimplified = Evaluator.SimplifyExp(this);
            Expression otherSimplified = Evaluator.SimplifyExp(other);
            Expression thisEvaluated = thisSimplified.Evaluate();
            Expression otherEvaluated = otherSimplified.Evaluate();

            if (thisEvaluated is Error && otherEvaluated is Error)
            {
                if (thisSimplified is Operator && otherEvaluated is Operator)
                {
                    return (thisSimplified as Operator).left.CompareTo((otherEvaluated as Operator).left) && (thisSimplified as Operator).right.CompareTo((otherEvaluated as Operator).right);
                }
                else
                {
                    return false;
                }
            }
            else if (thisEvaluated is Error)
            {
                return false;
            }
            else if (otherEvaluated is Error)
            {
                return false;
            }

            return thisEvaluated.CompareTo(otherEvaluated);
        }

        public override bool ContainsNotNumber(NotNumber other)
        {
            return left.ContainsNotNumber(other) || right.ContainsNotNumber(other);
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return Evaluate() + other;
        }

        public override Expression AddWith(Rational other)
        {
            return Evaluate() + other;
        }

        public override Expression AddWith(Irrational other)
        {
            return Evaluate() + other;
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return Evaluate() - other;
        }

        public override Expression SubWith(Rational other)
        {
            return Evaluate() - other;
        }

        public override Expression SubWith(Irrational other)
        {
            return Evaluate() - other;
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return Evaluate() * other;
        }

        public override Expression MulWith(Rational other)
        {
            return Evaluate() * other;
        }

        public override Expression MulWith(Irrational other)
        {
            return Evaluate() * other;
        }

        #endregion

        #region DivWith
        public override Expression DivWith(Integer other)
        {
            return Evaluate() / other;
        }

        public override Expression DivWith(Rational other)
        {
            return Evaluate() / other;
        }

        public override Expression DivWith(Irrational other)
        {
            return Evaluate() / other;
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return Evaluate() ^ other;
        }

        public override Expression ExpWith(Rational other)
        {
            return Evaluate() ^ other;
        }

        public override Expression ExpWith(Irrational other)
        {
            return Evaluate() ^ other;
        }

        #endregion
    }

    public class Equal : Operator
    {
        public Equal() : this(null, null) { }
        public Equal(Expression left, Expression right) : base(left, right)
        {
            symbol = "=";
            priority = 0;
        }

        public override Expression Expand()
        {
            var res = new Equal(left.Expand(), right.Expand());
            res.parent = parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression res = null, evaluatedLeft, evaluatedRight;

            res = new Equal(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if (!((evaluatedLeft = (res as Equal).left.Evaluate()) is Error))
            {
                (res as Equal).left = evaluatedLeft;
            }

            if (!((evaluatedRight = (res as Equal).right.Evaluate()) is Error))
            {
                (res as Equal).right = evaluatedRight;
            }

            res.parent = parent;
            return res;
        }
    }

    public class Assign : Operator
    {
        public Assign() : this(null, null) { }
        public Assign(Expression left, Expression right) : base(left, right)
        {
            symbol = ":=";
            priority = 0;
        }

        public override Expression Expand()
        {
            var res = new Assign(left.Expand(), right.Expand());
            res.parent = parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression res = null, evaluatedLeft, evaluatedRight;

            res = new Assign(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if (!((evaluatedLeft = (res as Assign).left.Evaluate()) is Error))
            {
                (res as Assign).left = evaluatedLeft;
            }

            if (!((evaluatedRight = (res as Assign).right.Evaluate()) is Error))
            {
                (res as Assign).right = evaluatedRight;
            }

            res.parent = parent;
            return res;
        }
    }

    public class Add : Operator
    {
        public Add() : this(null, null) { }
        public Add(Expression left, Expression right) : base(left, right)
        {
            symbol = "+";
            priority = 20;
        }

        public override Expression Evaluate ()
        {
            return left + right;

            /*
            if (left is Integer && right is Integer)
            {
                return new Integer((left as Integer).value + (right as Integer).value);
            }

            if (left is Integer && right is Rational)
            {
                return new Add(new Rational((left as Integer), new Integer(1)), right).Evaluate();
            }

            if (left is Rational && right is Integer)
            {
                return new Add(left, new Rational((right as Integer), new Integer(1))).Evaluate();
            }


            if (left is Rational && right is Rational)
            {
                var leftNumerator = new Integer((left as Rational).numerator.value * (right as Rational).denominator.value);
                var rightNumerator = new Integer((right as Rational).numerator.value * (left as Rational).denominator.value);

                return new Rational(new Add(leftNumerator, rightNumerator).Evaluate() as Integer,
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

            return base.Evaluate();
            */
        }

        public override Expression Expand()
        {
            var res = new Add(left.Expand(), right.Expand());
            res.parent = parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedLeft, evaluatedRight, res = null;
            Operator simplifiedOperator = new Add(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if (simplifiedOperator.left is Number && simplifiedOperator.left.CompareTo(new Integer(0)))
            {
                res = simplifiedOperator.right;
            }
            else if (simplifiedOperator.right is Number && simplifiedOperator.right.CompareTo(new Integer(0)))
            {
                res = simplifiedOperator.left;
            }
            else if (simplifiedOperator.left is Add)
            {
                res = (simplifiedOperator.left as Add).SimplifyMultiAdd(simplifiedOperator.right);
            }
            else if (simplifiedOperator.right is Add)
            {
                res = (simplifiedOperator.right as Add).SimplifyMultiAdd(simplifiedOperator.left);
            }
            else if (simplifiedOperator.left is NotNumber && simplifiedOperator.right is NotNumber && ((simplifiedOperator.left as NotNumber).identifier == (simplifiedOperator.right as NotNumber).identifier && (simplifiedOperator.left as NotNumber).exponent.CompareTo((simplifiedOperator.right as NotNumber).exponent)))
            {
                res = NotNumberOperation(simplifiedOperator.left as NotNumber, simplifiedOperator.right as NotNumber);
            }
            else
            {
                res = simplifiedOperator;
            }
            //else if ((res as Operator).left.CompareTo((res as Operator).right))
            //{
            //    res = new Mul(new Integer(2), (res as Operator).left);
            //}

            if (res is Add && !((evaluatedLeft = (res as Add).left.Evaluate()) is Error))
            {
                (res as Add).left = evaluatedLeft;
            }

            if (res is Add && !((evaluatedRight = (res as Add).right.Evaluate()) is Error))
            {
                (res as Add).right = evaluatedRight;
            }

            res.parent = parent;
            return res;
        }

        private Expression SimplifyMultiAdd(Expression other)
        {
            Expression res = null;

            if (other is Number)
            {
                if (left is Number)
                {
                    res = new Add(new Add(left, other).Evaluate(), right);
                }
                else if (right is Number)
                {
                    res = new Add(left, new Add(right, other).Evaluate());
                }
                else
                {
                    res = new Add(this, other);
                }
            }/*
            else if (other is Add)
            {
                res = new Add(SimplifyMultiAdd(left, (other as Operator).left), SimplifyMultiAdd(left, (other as Operator).right));
            }*/
            else if (other is NotNumber)
            {
                if (left is NotNumber && ((left as NotNumber).identifier == (other as NotNumber).identifier && (left as NotNumber).exponent.CompareTo((other as NotNumber).exponent)))
                {
                    res = new Add(NotNumberOperation(left as NotNumber, other as NotNumber), right);
                }
                else if (right is NotNumber && ((right as NotNumber).identifier == (other as NotNumber).identifier && (right as NotNumber).exponent.CompareTo((other as NotNumber).exponent)))
                {
                    res = new Add(left, NotNumberOperation(right as NotNumber, other as NotNumber));
                }
                else if (left is Add)
                {
                    res = new Add((left as Add).SimplifyMultiAdd(other), right);

                    if (res.ToString() == new Add(new Add(left, other), right).ToString())
                    {
                        res = new Add(this, other);
                    }
                }
                else if (right is Add)
                {
                    res = new Add(left, (right as Add).SimplifyMultiAdd(other));

                    if (res.ToString() == new Add(left, new Add(right, other)).ToString())
                    {
                        res = new Add(this, other);
                    }
                }
                else
                {
                    res = new Add(this, other);
                }
            }
            else
            {
                if (left.CompareTo(other))
                {
                    res = new Add(Evaluator.SimplifyExp(new Mul(new Integer(2), other)), right);
                }
                else if (right.CompareTo(other))
                {
                    res = new Add(left, Evaluator.SimplifyExp(new Mul(new Integer(2), other)));
                }
                else
                {
                    res = new Add(this, other);
                }
            }

            res.parent = parent;
            return res;
        }

        private Expression NotNumberOperation(NotNumber left, NotNumber right)
        {
            NotNumber res = left.Clone();

            res.prefix = (left.prefix + right.prefix) as Number;

            res.parent = parent;
            return res;
        }
    }

    public class Sub : Operator
    {
        public Sub() : this(null, null) { }
        public Sub(Expression left, Expression right) : base(left, right)
        {
            symbol = "-";
            priority = 20;
        }

        public override Expression Evaluate ()
        {
            return left - right;

            /*
            if (left is Integer && right is Integer) 
            {
                return new Integer((left as Integer).value - (right as Integer).value);
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
                var leftNumerator = new Integer((left as Rational).numerator.value * (right as Rational).denominator.value);
                var rightNumerator = new Integer((right as Rational).numerator.value * (left as Rational).denominator.value);

                return new Rational(new Sub(leftNumerator, rightNumerator).Evaluate() as Integer,
                                    new Mul((right as Rational).denominator, (left as Rational).denominator).Evaluate() as Integer);
            }

            if (left is Integer && right is Irrational) 
            {
                return new Irrational((left as Integer).value - (right as Irrational).value);
            }

            if (left is Irrational && right is Integer) 
            {
                return new Irrational((left as Irrational).value - (right as Integer).value);
            }

            if (left is Irrational && right is Irrational) 
            {
                return new Irrational((left as Irrational).value - (right as Irrational).value);
            }

            if (left is Irrational && right is Rational) 
            {
                return new Irrational((left as Irrational).value - (right as Rational).value.value);
            }

            if (left is Rational && right is Irrational) 
            {
                return new Irrational((left as Rational).value.value - (right as Irrational).value);
            }

            return base.Evaluate();
            */
        }

        public override Expression Expand()
        {
            var res = new Sub(left.Expand(), right.Expand());
            res.parent = parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression res = null;

            right = Evaluator.SimplifyExp(new Mul(new Integer(-1), right));
            res = new Add(left, right).Simplify();

            res.parent = parent;
            return res;
        }
    }

    public class Mul : Operator
    {
        public Mul() : this(null, null) { }
        public Mul(Expression left, Expression right) : base(left, right)
        {
            symbol = "*";
            priority = 30;
        }

        public override Expression Evaluate ()
        {
            return left * right;

            /*
            if (left is Integer && right is Integer) 
            {
                return new Integer((left as Integer).value * (right as Integer).value);
            }

            if (left is Integer && right is Rational) 
            {
                return new Mul(new Rational((left as Integer), new Integer(1)), right).Evaluate();
            }

            if (left is Rational && right is Integer) 
            {
                return new Mul(left, new Rational((right as Integer), new Integer(1))).Evaluate();
            }

            if (left is Rational && right is Rational) 
            {
                return new Rational(new Mul((left as Rational).numerator, (right as Rational).numerator).Evaluate() as Integer,
                                   new Mul((left as Rational).denominator, (right as Rational).denominator).Evaluate() as Integer);
            }

            if (left is Integer && right is Irrational)
            {
                return new Irrational((left as Integer).value * (right as Irrational).value);
            }

            if (left is Irrational && right is Integer) 
            {
                return new Irrational((left as Irrational).value * (right as Integer).value);
            }

            if (left is Irrational && right is Irrational) 
            {
                return new Irrational((left as Irrational).value * (right as Irrational).value);
            }

            if (left is Irrational && right is Rational) 
            {
                return new Irrational((left as Irrational).value * (right as Rational).value.value);
            }

            if (left is Rational && right is Irrational) 
            {
                return new Irrational((left as Rational).value.value * (right as Irrational).value);
            }

            return base.Evaluate();
            */
        }

        public override Expression Expand()
        {
            Expression res = null;

            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    res = new Add(new Mul((left as Operator).left, right), new Mul((left as Operator).right, right));
                } 
                else if (left is Sub)
                {
                    res = new Sub(new Mul((left as Operator).left, right), new Mul((left as Operator).right, right));
                }
            } 
            else if (right is Operator && (right as Operator).priority < priority)
            {
                if (right is Add)
                {
                    res = new Add(new Mul((right as Operator).left, left), new Mul((right as Operator).right, left));
                }
                else if (right is Sub)
                {
                    res = new Sub(new Mul((right as Operator).left, left), new Mul((right as Operator).right, left));
                }
            }
            else
            {
                res = new Mul(left.Expand(), right.Expand());
            }

            res.parent = parent;
            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedLeft, evaluatedRight, res = null;
            Operator simplifiedOperator = new Mul(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if (simplifiedOperator.left is Mul)
            {
                res = (simplifiedOperator.left as Mul).SimplifyMultiMul(simplifiedOperator.right);
            }
            else if (simplifiedOperator.right is Mul)
            {
                res = (simplifiedOperator.right as Mul).SimplifyMultiMul(simplifiedOperator.left);
            }
            else if (simplifiedOperator.left is NotNumber && simplifiedOperator.right is Number)
            {
                res = simplifiedOperator.left;
                (res as NotNumber).prefix = new Mul((res as NotNumber).prefix, simplifiedOperator.right).Evaluate() as Number;
            }
            else if (simplifiedOperator.left is Number && simplifiedOperator.right is NotNumber)
            {
                res = simplifiedOperator.right;
                (res as NotNumber).prefix = new Mul((res as NotNumber).prefix, simplifiedOperator.left).Evaluate() as Number;
            }
            else if (simplifiedOperator.left is NotNumber && simplifiedOperator.right is NotNumber && (simplifiedOperator.left as NotNumber).identifier == (simplifiedOperator.right as NotNumber).identifier)
            {
                res = NotNumberOperation(simplifiedOperator.left as NotNumber, simplifiedOperator.right as NotNumber);
            }
            else
            {
                res = simplifiedOperator;
            }
            //else if ((res as Operator).left.CompareTo((res as Operator).right))
            //{
            //    res = new Mul(new Integer(2), (res as Operator).left);
            //}

            if (res is Mul && !((evaluatedLeft = (res as Mul).left.Evaluate()) is Error))
            {
                (res as Mul).left = evaluatedLeft;
            }

            if (res is Mul && !((evaluatedRight = (res as Mul).right.Evaluate()) is Error))
            {
                (res as Mul).right = evaluatedRight;
            }

            res.parent = parent;
            return res;
        }

        private Expression SimplifyMultiMul(Expression other)
        {
            Expression res = null;

            if (other is Number)
            {
                if (left is Number)
                {
                    res = new Mul(new Mul(left, other).Evaluate(), right);
                }
                else if (right is Number)
                {
                    res = new Mul(left, new Mul(right, other).Evaluate());
                }
                else
                {
                    res = new Mul(this, other);
                }
            }/*
            else if (other is Add)
            {
                res = new Add(SimplifyMultiAdd(left, (other as Operator).left), SimplifyMultiAdd(left, (other as Operator).right));
            }*/
            else if (other is NotNumber)
            {
                if (left is NotNumber && (left as NotNumber).identifier == (other as NotNumber).identifier)
                {
                    res = new Mul(NotNumberOperation(left as NotNumber, other as NotNumber), right);
                }
                else if (right is NotNumber && (right as NotNumber).identifier == (other as NotNumber).identifier)
                {
                    res = new Mul(left, NotNumberOperation(right as NotNumber, other as NotNumber));
                }
                else if (left is Mul)
                {
                    res = new Mul((left as Mul).SimplifyMultiMul(other), right);

                    if (res.ToString() == new Mul(new Mul(left, other), right).ToString())
                    {
                        res = new Mul(this, other);
                    }
                }
                else if (right is Mul)
                {
                    res = new Mul(left, (right as Mul).SimplifyMultiMul(other));

                    if (res.ToString() == new Mul(left, new Mul(right, other)).ToString())
                    {
                        res = new Mul(this, other);
                    }
                }
                else
                {
                    res = new Mul(this, other);
                }
            }
            else
            {
                if (left.CompareTo(other))
                {
                    res = new Mul(Evaluator.SimplifyExp(new Exp(other, new Integer(2))), right);
                }
                else if (right.CompareTo(other))
                {
                    res = new Mul(left, Evaluator.SimplifyExp(new Mul(other, new Integer(2))));
                }
                else
                {
                    res = new Mul(this, other);
                }
            }

            res.parent = parent;
            return res;
        }

        private NotNumber NotNumberOperation(NotNumber left, NotNumber right)
        {
            NotNumber res = left.Clone();

            res.prefix = (left.prefix * right.prefix) as Number;
            res.exponent = (left.exponent + right.exponent) as Number;

            return res;
        }

    }

    public class Div : Operator
    {
        public Div() : this(null, null) { }
        public Div(Expression left, Expression right) : base(left, right)
        {
            symbol = "/";
            priority = 30;
        }

        public override Expression Evaluate()
        {
            return left / right;

            /*
            if (left is Integer && right is Integer)
            {
                if ((right as Integer).value != 0)
                    return new Rational((left as Integer), (right as Integer));
                else
                    return new Error(this, "Cannot divide by zero");
            }

            if (left is Integer && right is Rational)
            {
                return new Div(new Rational((left as Integer), new Integer(1)), right).Evaluate();
            }

            if (left is Rational && right is Integer)
            {
                return new Div(left, new Rational((right as Integer), new Integer(1))).Evaluate();
            }

            if (left is Rational && right is Rational)
            {
                return new Mul(new Rational((left as Rational).denominator, (left as Rational).numerator), right).Evaluate();
            }

            if (left is Integer && right is Irrational)
            {
                return new Irrational((left as Integer).value / (right as Irrational).value);
            }

            if (left is Irrational && right is Integer)
            {
                return new Irrational((left as Irrational).value / (right as Integer).value);
            }

            if (left is Irrational && right is Irrational)
            {
                return new Irrational((left as Irrational).value / (right as Irrational).value);
            }

            if (left is Irrational && right is Rational)
            {
                return new Irrational((left as Irrational).value / (right as Rational).value.value);
            }

            if (left is Rational && right is Irrational)
            {
                return new Irrational((left as Rational).value.value / (right as Irrational).value);
            }

            return base.Evaluate();
            */
        }

        public override Expression Expand()
        {
            Expression res = null;

            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    res = new Add(new Div((left as Operator).left, right), new Div((left as Operator).right, right));
                }
                else if (left is Sub)
                {
                    res = new Sub(new Div((left as Operator).left, right), new Div((left as Operator).right, right));
                }
            }
            else if (right is Operator && (right as Operator).priority < priority)
            {
                if (right is Add)
                {
                    res = new Add(new Div((right as Operator).left, left), new Div((right as Operator).right, left));
                }
                else if (right is Sub)
                {
                    res = new Sub(new Div((right as Operator).left, left), new Div((right as Operator).right, left));
                }
            }
            else
            {
                res = new Div(left.Expand(), right.Expand());
            }

            res.parent = parent;
            return res;
        }

        public override Expression Simplify()
        {
            Expression res = null, evaluatedLeft, evaluatedRight;

            if ((left is Symbol && right is Symbol) && ((left as Symbol).identifier == (right as Symbol).identifier))
            {
                //res = FunctionOrSymbol<Symbol>(left, right);
            }
            else if ((left is Function && right is Function) && ((left as Function).identifier == (right as Function).identifier && (left as Function).CompareArgsTo(right as Function)))
            {
                //NewFunction(left, right, ref res);
            }
            else
            {
                res = new Div(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

                if (!((evaluatedLeft = (res as Div).left.Evaluate()) is Error))
                {
                    (res as Div).left = evaluatedLeft;
                }

                if (!((evaluatedRight = (res as Div).right.Evaluate()) is Error))
                {
                    (res as Div).right = evaluatedRight;
                }
            }

            res.parent = parent;
            return res;
        }
    }

    public class Exp : Operator
    {
        public Exp() : this(null, null) { }
        public Exp(Expression left, Expression right) : base(left, right)
        {
            symbol = "^";
            priority = 40;
        }

        public override Expression Evaluate()
        {
            return left ^ right;

            /*
            if (left is Integer && right is Integer) 
            {
                return new Integer( (Int64)Math.Pow((left as Integer).value, (right as Integer).value));
            }

            if (left is Integer && right is Rational)
            {
                return new Exp(new Rational((left as Integer), new Integer(1)), right).Evaluate();
            }

            if (left is Rational && right is Integer)
            {
                return new Exp(left, new Rational((right as Integer), new Integer(1))).Evaluate();
            }

            if (left is Rational && right is Rational)
            {
                return new Irrational((decimal)Math.Pow((double)(left as Rational).value.value, (double)(left as Rational).value.value));
            }

            if (left is Integer && right is Irrational)
            {
                return new Irrational((decimal)Math.Pow((left as Integer).value, (double)(right as Irrational).value));
            }

            if (left is Irrational && right is Integer)
            {
                return new Irrational((decimal)Math.Pow((double)(left as Irrational).value, (right as Integer).value));
            }

            if (left is Irrational && right is Irrational)
            {
                return new Irrational((decimal)Math.Pow((double)(left as Irrational).value, (double)(right as Irrational).value));
            }

            if (left is Irrational && right is Rational)
            {
                return new Irrational((decimal)Math.Pow((double)(left as Irrational).value, (double)(right as Rational).value.value));
            }

            if (left is Rational && right is Irrational)
            {
                return new Irrational((decimal)Math.Pow((double)(left as Rational).value.value, (double)(right as Irrational).value));
            }

            return base.Evaluate();
            */
        }

        public override Expression Expand()
        {
            Expression res = null;

            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    res = new Add(new Add(new Exp((left as Operator).left, right), new Exp((left as Operator).right, right)), new Mul(new Integer(2), new Mul((left as Operator).left, (left as Operator).right)));
                } 
                else if (left is Sub)
                {
                    res = new Sub(new Add(new Exp((left as Operator).left, right), new Exp((left as Operator).right, right)), new Mul(new Integer(2), new Mul((left as Operator).left, (left as Operator).right)));
                }
                else if (left is Mul)
                {
                    res = new Mul(new Exp((left as Operator).left, right), new Exp((left as Operator).right, right)); 
                }
                else if (left is Div)
                {
                    res = new Div(new Exp((left as Operator).left, right), new Exp((left as Operator).right, right)); 
                }
            }
            else
            {
                res = new Exp(left.Expand(), right.Expand());
            }

            res.parent = parent;
            return res;
        }

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (Evaluate() is Error || other.Evaluate() is Error) 
                {
                    if (left.CompareTo((other as Exp).left) && right.CompareTo((other as Exp).right)) 
                    {
                        res = true;
                    }
                }
                else if (Evaluate().CompareTo(other.Evaluate()))
                {
                    res = true;
                }
            }

            return res;
        }
    }
}