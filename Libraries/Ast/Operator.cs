using System;

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
            if ((left is Operator || left is Symbol) || (right is Operator || left is Symbol))
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
            return (new BooleanEqual(this.Evaluate(), other.Evaluate()).Evaluate() as Boolean).value;
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
            res.parent = this.parent;

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
            res.parent = this.parent;

            return res;
        }

        public override Expression Simplify()
        {
            var res = new Assign(left.Evaluate(), right.Evaluate());
            res.parent = this.parent;

            if (res.left is Error)
            {
                res.left = this.left.Simplify();
            }

            if (res.right is Error)
            {
                res.right = this.right.Simplify();
            }

            return res;
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
            res.parent = this.parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression res = new Add(left.Evaluate(), right.Evaluate());
            res.parent = this.parent;

            if ((res as Operator).left is Error && (res as Operator).right is Error)
            {
                res = this;

                if ((this.left is Symbol && this.right is Symbol) && (((this.left as Symbol).symbol == (this.right as Symbol).symbol) && ((new BooleanEqual((this.left as Symbol).exponent, (this.left as Symbol).exponent).Evaluate() as Boolean).value)))
                {
                    res = new Symbol((this.left as Symbol).evaluator, (this.left as Symbol).symbol, (new Add((this.left as Symbol).prefix, (this.right as Symbol).prefix).Evaluate() as Number), (this.left as Symbol).exponent);

                    if ((new BooleanEqual((res as Symbol).prefix, new Integer(0)).Evaluate() as Boolean).value)
                    {
                        res = new Integer(0);
                    }
                }
                else if ((this.left is Function && this.right is Function) && ((this.left as Function).identifier == (this.right as Function).identifier && (this.left as Function).CompareArgsTo(this.right as Function) && ((new BooleanEqual((this.left as Function).exponent, (this.left as Function).exponent).Evaluate() as Boolean).value)))
                {
                    if (this.left is UserDefinedFunction && this.right is UserDefinedFunction)
                    {
                        res = new UserDefinedFunction((this.left as UserDefinedFunction).identifier, (this.left as UserDefinedFunction).args, (new Add((this.left as UserDefinedFunction).prefix, (this.right as UserDefinedFunction).prefix).Evaluate() as Number), (this.left as UserDefinedFunction).exponent);
                    }
                    else if (this.left is Sin && this.right is Sin)
                    {
                        res = new Sin((this.left as Sin).identifier, (this.left as Sin).args[0], (new Add((this.left as Sin).prefix, (this.right as Sin).prefix).Evaluate() as Number), (this.left as Sin).exponent);
                    }
                    else if (this.left is ASin && this.right is ASin)
                    {
                        res = new ASin((this.left as ASin).identifier, (this.left as ASin).args[0], (new Add((this.left as ASin).prefix, (this.right as ASin).prefix).Evaluate() as Number), (this.left as ASin).exponent);
                    }
                    else if (this.left is Cos && this.right is Cos)
                    {
                        res = new Cos((this.left as Cos).identifier, (this.left as Cos).args[0], (new Add((this.left as Cos).prefix, (this.right as Cos).prefix).Evaluate() as Number), (this.left as Cos).exponent);
                    }
                    else if (this.left is ACos && this.right is ACos)
                    {
                        res = new ACos((this.left as ACos).identifier, (this.left as ACos).args[0], (new Add((this.left as ACos).prefix, (this.right as ACos).prefix).Evaluate() as Number), (this.left as ACos).exponent);
                    }
                    else if (this.left is Tan && this.right is Tan)
                    {
                        res = new Tan((this.left as Tan).identifier, (this.left as Tan).args[0], (new Add((this.left as Tan).prefix, (this.right as Tan).prefix).Evaluate() as Number), (this.left as Tan).exponent);
                    }
                    else if (this.left is ATan && this.right is ATan)
                    {
                        res = new ATan((this.left as ATan).identifier, (this.left as ATan).args[0], (new Add((this.left as ATan).prefix, (this.right as ATan).prefix).Evaluate() as Number), (this.left as ATan).exponent);
                    }
                    else if (this.left is Sqrt && this.right is Sqrt)
                    {
                        res = new Sqrt((this.left as Sqrt).identifier, (this.left as Sqrt).args[0], (new Add((this.left as Sqrt).prefix, (this.right as Sqrt).prefix).Evaluate() as Number), (this.left as Sqrt).exponent);
                    }

                    if ((new BooleanEqual((res as Function).prefix, new Integer(0)).Evaluate() as Boolean).value)
                    {
                        res = new Integer(0);
                    }
                }
                else
                {
                    res = new Add(left.Simplify(), right.Simplify());
                }
            }
            else if ((res as Operator).left is Error)
            {
                (res as Operator).left = this.left.Simplify();
            }
            else if ((res as Operator).right is Error)
            {
                (res as Operator).right = this.right.Simplify();
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
            priority = 10;
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
            res.parent = this.parent;

            return res;
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

            if (left is Operator && (left as Operator).priority < this.priority)
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
            else if (right is Operator && (right as Operator).priority < this.priority)
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

            res.parent = this.parent;
            return res;
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

            if (left is Operator && (left as Operator).priority < this.priority)
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
            else if (right is Operator && (right as Operator).priority < this.priority)
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

            res.parent = this.parent;
            return res;
        }

        public override Expression Simplify()
        {
            Expression res = new Div(left.Evaluate(), right.Evaluate());
            res.parent = this.parent;

            if ((res as Operator).left is Error && (res as Operator).right is Error)
            {
                res = this;

                if ((this.left is Symbol && this.right is Symbol) && ((this.left as Symbol).symbol == (this.right as Symbol).symbol))
                {
                    res = new Symbol((this.left as Symbol).evaluator, (this.left as Symbol).symbol, (new Div((this.left as Symbol).prefix, (this.right as Symbol).prefix).Evaluate() as Number), (new Div((this.left as Symbol).exponent, (this.right as Symbol).exponent).Evaluate() as Number));
                    
                    return res;
                }

                if ((this.left is Function && this.right is Function) && ((this.left as Function).identifier == (this.right as Function).identifier && (this.left as Function).CompareArgsTo(this.right as Function)))
                {
                    if (this.left is UserDefinedFunction && this.right is UserDefinedFunction)
                    {
                        res = new UserDefinedFunction((this.left as Function).identifier, (this.left as Function).args, (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }
                    else if (this.left is Sin && this.right is Sin)
                    {
                        res = new Sin((this.left as Function).identifier, (this.left as Function).args[0], (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }
                    else if (this.left is ASin && this.right is ASin)
                    {
                        res = new ASin((this.left as Function).identifier, (this.left as Function).args[0], (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }
                    else if (this.left is Cos && this.right is Cos)
                    {
                        res = new Cos((this.left as Function).identifier, (this.left as Function).args[0], (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }
                    else if (this.left is ACos && this.right is ACos)
                    {
                        res = new ACos((this.left as Function).identifier, (this.left as Function).args[0], (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }
                    else if (this.left is Tan && this.right is Tan)
                    {
                        res = new Tan((this.left as Function).identifier, (this.left as Function).args[0], (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }
                    else if (this.left is ATan && this.right is ATan)
                    {
                        res = new ATan((this.left as Function).identifier, (this.left as Function).args[0], (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }
                    else if (this.left is Sqrt && this.right is Sqrt)
                    {
                        res = new Sqrt((this.left as Function).identifier, (this.left as Function).args[0], (new Div((this.left as Function).prefix, (this.right as Function).prefix).Evaluate() as Number), (new Div((this.left as Function).exponent, (this.right as Function).exponent).Evaluate() as Number));
                    }

                    return res;
                }
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
            priority = 30;
        }

        public override Expression Evaluate()
        {
            if (left is Integer && right is Integer) 
            {
                return new Integer( (int)Math.Pow((left as Integer).value, (right as Integer).value));
            }

            return base.Evaluate();
        }

        public override Expression Expand()
        {
            Expression res = null;

            if (left is Operator && (left as Operator).priority < this.priority)
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

            res.parent = this.parent;
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