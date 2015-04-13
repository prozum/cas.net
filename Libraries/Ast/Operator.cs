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
            if (left is Operator || left is Symbol || left is Function || right is Operator || right is Symbol || right is Function)
            {
                if (this is Add)
                {
                    return new Add(left.Evaluate(), right.Evaluate()).Evaluate();
                }

                if (this is Sub)
                {
                    return new Sub(left.Evaluate(), right.Evaluate()).Evaluate();
                }

                if (this is Mul)
                {
                    return new Mul(left.Evaluate(), right.Evaluate()).Evaluate();
                }

                if (this is Div)
                {
                    return new Div(left.Evaluate(), right.Evaluate()).Evaluate();
                }

                if (this is Exp)
                {
                    return new Exp(left.Evaluate(), right.Evaluate()).Evaluate();
                }
            }

            return new Error("Cannot evaluate operator expression!");
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
            Expression thisEvaluated = Evaluator.SimplifyExp(Evaluate());
            Expression otherEvaluated = Evaluator.SimplifyExp(other.Evaluate());

            return thisEvaluated.CompareTo(otherEvaluated);
        }

        protected void NewFunction(Expression left, Expression right, ref Expression res)
        {
            if (left is UserDefinedFunction && right is UserDefinedFunction)
            {
                res = FunctionOrSymbol<UserDefinedFunction>(left, right);
            }
            else if (left is Sin && right is Sin)
            {
                res = FunctionOrSymbol<Sin>(left, right);
            }
            else if (left is ASin && right is ASin)
            {
                res = FunctionOrSymbol<ASin>(left, right);
            }
            else if (left is Cos && right is Cos)
            {
                res = FunctionOrSymbol<Cos>(left, right);
            }
            else if (left is ACos && right is ACos)
            {
                res = FunctionOrSymbol<ACos>(left, right);
            }
            else if (left is Tan && right is Tan)
            {
                res = FunctionOrSymbol<Tan>(left, right);
            }
            else if (left is ATan && right is ATan)
            {
                res = FunctionOrSymbol<ATan>(left, right);
            }
            else if (left is Sqrt && right is Sqrt)
            {
                res = FunctionOrSymbol<Sqrt>(left, right);
            }
        }

        protected virtual Expression FunctionOrSymbol<T>(Expression left, Expression right) where T : Expression, new()
        {
            var res = new T();

            return res;
        }
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

            if (!((evaluatedLeft = (res as Operator).left.Evaluate()) is Error))
            {
                (res as Operator).left = evaluatedLeft;
            }

            if (!((evaluatedRight = (res as Operator).right.Evaluate()) is Error))
            {
                (res as Operator).right = evaluatedRight;
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

            if (!((evaluatedLeft = (res as Operator).left.Evaluate()) is Error))
            {
                (res as Operator).left = evaluatedLeft;
            }

            if (!((evaluatedRight = (res as Operator).right.Evaluate()) is Error))
            {
                (res as Operator).right = evaluatedRight;
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
        }

        public override Expression Expand()
        {
            var res = new Add(left.Expand(), right.Expand());
            res.parent = parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedLeft, evaluatedRight;
            Expression res = new Add(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if ((res as Operator).left is Add)
            {
                res = ((res as Operator).left as Add).SimplifyMultiAdd((res as Operator).right);
            }
            else if ((res as Operator).right is Add)
            {
                res = ((res as Operator).right as Add).SimplifyMultiAdd((res as Operator).left);
            }
            else if (((res as Operator).left is Symbol && (res as Operator).right is Symbol) && ((((res as Operator).left as Symbol).symbol == ((res as Operator).right as Symbol).symbol) && ((res as Operator).left as Symbol).exponent.CompareTo(((res as Operator).right as Symbol).exponent)))
            {
                res = FunctionOrSymbol<Symbol>((res as Operator).left, (res as Operator).right);
            }
            else if (((res as Operator).left is Function && (res as Operator).right is Function) && (((res as Operator).left as Function).identifier == ((res as Operator).right as Function).identifier && ((res as Operator).left as Function).CompareArgsTo((res as Operator).right as Function) && ((res as Operator).left as Function).CompareTo(((res as Operator).right as Function))))
            {
                NewFunction(left, right, ref res);
            }
            //else if ((res as Operator).left.CompareTo((res as Operator).right))
            //{
            //    res = new Mul(new Integer(2), (res as Operator).left);
            //}

            if (res is Operator && !((evaluatedLeft = (res as Operator).left.Evaluate()) is Error))
            {
                (res as Operator).left = evaluatedLeft;
            }

            if (res is Operator && !((evaluatedRight = (res as Operator).right.Evaluate()) is Error))
            {
                (res as Operator).right = evaluatedRight;
            }

            res.parent = parent;
            return res;
        }

        private Expression SimplifyMultiAdd(Expression other)
        {
            Expression res = null, function = null;

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
            else if (other is Symbol)
            {
                if (left is Symbol && ((left as Symbol).symbol == (other as Symbol).symbol && (left as Symbol).exponent.CompareTo((other as Symbol).exponent)))
                {
                    res = new Add(FunctionOrSymbol<Symbol>(left, other), right);
                }
                else if (right is Symbol && (right as Symbol).symbol == (other as Symbol).symbol && (right as Symbol).exponent.CompareTo((other as Symbol).exponent))
                {
                    res = new Add(left, FunctionOrSymbol<Symbol>(right, other));
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
            else if (other is Function)
            {
                if (left is Function && (left as Function).identifier == (other as Function).identifier && (left as Function).exponent.CompareTo((other as Function).exponent))
                {
                    NewFunction(left, other, ref function);
                    res = new Add(function, right);
                }
                else if (right is Function && ((right as Function).identifier == (other as Function).identifier && (right as Function).exponent.CompareTo((other as Function).exponent)))
                {
                    NewFunction(right, other, ref function);
                    res = new Add(left, function);
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
        
        protected override Expression FunctionOrSymbol<T>(Expression left, Expression right)
        {
            var res = new T();

            if (res is Function)
            {
                (res as Function).identifier = (left as Function).identifier;
                (res as Function).args = (left as Function).args;
                (res as Function).prefix = (new Add((left as Function).prefix, (right as Function).prefix).Evaluate() as Number);
                (res as Function).exponent = (left as Function).exponent;
            }
            else if(res is Symbol)
            {
                (res as Symbol).symbol = (left as Symbol).symbol;
                (res as Symbol).evaluator = (left as Symbol).evaluator;
                (res as Symbol).prefix = (new Add((left as Symbol).prefix, (right as Symbol).prefix).Evaluate() as Number);
                (res as Symbol).exponent = (left as Symbol).exponent;
            }
            else
            {
                throw new TypeLoadException("Neither function or symbol was used");
            }

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
        }

        public override Expression Expand()
        {
            var res = new Sub(left.Expand(), right.Expand());
            res.parent = parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression res = null, evaluatedLeft, evaluatedRight;

            if ((left is Symbol && right is Symbol) && (((left as Symbol).symbol == (right as Symbol).symbol) && ((new BooleanEqual((left as Symbol).exponent, (right as Symbol).exponent).Evaluate() as Boolean).value)))
            {
                res = FunctionOrSymbol<Symbol>(left, right);
            }
            else if ((left is Function && right is Function) && ((left as Function).identifier == (right as Function).identifier && (left as Function).CompareArgsTo(right as Function) && ((new BooleanEqual((left as Function).exponent, (right as Function).exponent).Evaluate() as Boolean).value)))
            {
                NewFunction(left, right, ref res);
            }
            else
            {
                res = new Sub(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

                if (!((evaluatedLeft = (res as Operator).left.Evaluate()) is Error))
                {
                    (res as Operator).left = evaluatedLeft;
                }

                if (!((evaluatedRight = (res as Operator).right.Evaluate()) is Error))
                {
                    (res as Operator).right = evaluatedRight;
                }
            }

            res.parent = parent;
            return res;
        }

        protected override Expression FunctionOrSymbol<T>(Expression left, Expression right)
        {
            var res = new T();

            if (res is Function)
            {
                (res as Function).identifier = (left as Function).identifier;
                (res as Function).args = (left as Function).args;
                (res as Function).prefix = (new Sub((left as Function).prefix, (right as Function).prefix).Evaluate() as Number);
                (res as Function).exponent = (left as Function).exponent;
            }
            else if (res is Symbol)
            {
                (res as Symbol).symbol = (left as Symbol).symbol;
                (res as Symbol).evaluator = (left as Symbol).evaluator;
                (res as Symbol).prefix = (new Sub((left as Symbol).prefix, (right as Symbol).prefix).Evaluate() as Number);
                (res as Symbol).exponent = (left as Symbol).exponent;
            }
            else
            {
                throw new TypeLoadException("Neither function or symbol was used");
            }

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
            Expression res = null, evaluatedLeft, evaluatedRight;

            if ((left is Symbol && right is Symbol) && ((left as Symbol).symbol == (right as Symbol).symbol))
            {
                res = FunctionOrSymbol<Symbol>(left, right);
            }
            else if ((left is Function && right is Function) && ((left as Function).identifier == (right as Function).identifier && (left as Function).CompareArgsTo(right as Function)))
            {
                NewFunction(left, right, ref res);
            }
            else
            {
                res = new Mul(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

                if (!((evaluatedLeft = (res as Operator).left.Evaluate()) is Error))
                {
                    (res as Operator).left = evaluatedLeft;
                }

                if (!((evaluatedRight = (res as Operator).right.Evaluate()) is Error))
                {
                    (res as Operator).right = evaluatedRight;
                }
            }

            res.parent = parent;
            return res;
        }

        protected override Expression FunctionOrSymbol<T>(Expression left, Expression right)
        {
            var res = new T();

            if (res is Function)
            {
                (res as Function).identifier = (left as Function).identifier;
                (res as Function).args = (left as Function).args;
                (res as Function).prefix = (new Mul((left as Function).prefix, (right as Function).prefix).Evaluate() as Number);
                (res as Function).exponent = (new Add((left as Function).exponent, (right as Function).exponent).Evaluate() as Number);
            }
            else if (res is Symbol)
            {
                (res as Symbol).symbol = (left as Symbol).symbol;
                (res as Symbol).evaluator = (left as Symbol).evaluator;
                (res as Symbol).prefix = (new Sub((left as Symbol).prefix, (right as Symbol).prefix).Evaluate() as Number);
                (res as Symbol).exponent = (new Add((left as Symbol).exponent, (right as Symbol).exponent).Evaluate() as Number);
            }
            else
            {
                throw new TypeLoadException("Neither function or symbol was used");
            }

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

        /* fix errors */
        public override Expression Evaluate()
        {
            if (left is Integer && right is Integer)
            {
                return new Rational((left as Integer), (right as Integer));
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

            if ((left is Symbol && right is Symbol) && ((left as Symbol).symbol == (right as Symbol).symbol))
            {
                res = FunctionOrSymbol<Symbol>(left, right);
            }
            else if ((left is Function && right is Function) && ((left as Function).identifier == (right as Function).identifier && (left as Function).CompareArgsTo(right as Function)))
            {
                NewFunction(left, right, ref res);
            }
            else
            {
                res = new Div(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

                if (!((evaluatedLeft = (res as Operator).left.Evaluate()) is Error))
                {
                    (res as Operator).left = evaluatedLeft;
                }

                if (!((evaluatedRight = (res as Operator).right.Evaluate()) is Error))
                {
                    (res as Operator).right = evaluatedRight;
                }
            }

            res.parent = parent;
            return res;
        }

        protected override Expression FunctionOrSymbol<T>(Expression left, Expression right)
        {
            var res = new T();

            if (res is Function)
            {
                (res as Function).identifier = (left as Function).identifier;
                (res as Function).args = (left as Function).args;
                (res as Function).prefix = (new Div((left as Function).prefix, (right as Function).prefix).Evaluate() as Number);
                (res as Function).exponent = (new Sub((left as Function).exponent, (right as Function).exponent).Evaluate() as Number);
            }
            else if (res is Symbol)
            {
                (res as Symbol).symbol = (left as Symbol).symbol;
                (res as Symbol).evaluator = (left as Symbol).evaluator;
                (res as Symbol).prefix = (new Div((left as Symbol).prefix, (right as Symbol).prefix).Evaluate() as Number);
                (res as Symbol).exponent = (new Sub((left as Symbol).exponent, (right as Symbol).exponent).Evaluate() as Number);
            }
            else
            {
                throw new TypeLoadException("Neither function or symbol was used");
            }

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
            if (left is Integer && right is Integer) 
            {
                return new Integer( (int)Math.Pow((left as Integer).value, (right as Integer).value));
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