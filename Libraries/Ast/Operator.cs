using System;
using System.Collections.Generic;

namespace Ast
{
    public interface ISwappable
    {
        Operator Swap();
    }

    public abstract class Operator : Expression
    {
        public string symbol;
        public int priority;
        public Expression left,right;

        public Operator(string symbol, int priority)
        {
            this.left = null;
            this.right = null;
            this.symbol = symbol;
            this.priority = priority;
        }
        public Operator(Expression left, Expression right, string symbol, int priority)
        {
            this.left = left;
            this.right = right;
            this.left.parent = this;
            this.right.parent = this;
            this.symbol = symbol;
            this.priority = priority;
        }

        public void SetRight(Expression exp)
        {
            right = exp;
            right.parent = this;
        }

        public void SetLeft(Expression exp)
        {
            left = exp;
            left.parent = this;
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
        }

        public override bool CompareTo(Expression other)
        {
            Expression thisSimplified = Evaluator.SimplifyExp(this);
            Expression otherSimplified = Evaluator.SimplifyExp(other);
            Expression thisEvaluated = thisSimplified.Evaluate();
            Expression otherEvaluated = otherSimplified.Evaluate();

            if (thisEvaluated is Error && otherEvaluated is Error)
            {
                if (thisSimplified is Operator && otherSimplified is Operator)
                {
                    if (thisSimplified is ISwappable)
                    {
                        return CompareSides(thisSimplified as Operator, otherSimplified as Operator) || CompareSides((thisSimplified as ISwappable).Swap(), otherSimplified as Operator);
                    }
                    else
                    {
                        return CompareSides(thisSimplified as Operator, otherSimplified as Operator);
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (thisEvaluated is Error || otherEvaluated is Error)
            {
                return false;
            }

            return thisEvaluated.CompareTo(otherEvaluated);
        }

        private bool CompareSides(Operator exp1, Operator exp2)
        {
            return exp1.left.CompareTo(exp2.left) && exp1.right.CompareTo(exp2.right);
        }

        public override bool ContainsVariable(Variable other)
        {
            return left.ContainsVariable(other) || right.ContainsVariable(other);
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

        #region GreaterThan
        public override Expression GreaterThan(Integer other)
        {
            return Evaluate() > other;
        }

        public override Expression GreaterThan(Rational other)
        {
            return Evaluate() > other;
        }

        public override Expression GreaterThan(Irrational other)
        {
            return Evaluate() > other;
        }

        #endregion

        #region LesserThan
        public override Expression LesserThan(Integer other)
        {
            return Evaluate() < other;
        }

        public override Expression LesserThan(Rational other)
        {
            return Evaluate() < other;
        }

        public override Expression LesserThan(Irrational other)
        {
            return Evaluate() < other;
        }

        #endregion

        #region GreaterThanOrEqualTo
        public override Expression GreaterThanOrEqualTo(Integer other)
        {
            return Evaluate() >= other;
        }

        public override Expression GreaterThanOrEqualTo(Rational other)
        {
            return Evaluate() >= other;
        }

        public override Expression GreaterThanOrEqualTo(Irrational other)
        {
            return Evaluate() >= other;
        }

        #endregion

        #region LesserThanOrEqualTo
        public override Expression LesserThanOrEqualTo(Integer other)
        {
            return Evaluate() <= other;
        }

        public override Expression LesserThanOrEqualTo(Rational other)
        {
            return Evaluate() <= other;
        }

        public override Expression LesserThanOrEqualTo(Irrational other)
        {
            return Evaluate() <= other;
        }

        #endregion
    }

    public class Equal : Operator
    {
        public Equal() : base("=", 0) { }
        public Equal(Expression left, Expression right) : base(left, right, "=", 0) { }

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

        public override Expression Clone()
        {
            return new Equal(left.Clone(), right.Clone());
        }
    }

    public class Assign : Operator
    {
        public Assign() : base(":=", 0) { }
        public Assign(Expression left, Expression right) : base(left, right, ":=", 0) { }

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

        public override Expression Clone()
        {
            return new Assign(left.Clone(), right.Clone());
        }
    }

    public class Add : Operator, ISwappable, IInvertable
    {
        public Add() : base("+", 20) { }
        public Add(Expression left, Expression right) : base(left, right, "+", 20) { }

        public override Expression Evaluate ()
        {
            return left + right;
        }

        public override Expression Expand()
        {
            var res = new Add(left.Expand(), right.Expand());
            res.parent = parent;

            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedRes, res = null;
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
            else if (simplifiedOperator.left is Variable && simplifiedOperator.right is Variable && ((simplifiedOperator.left as Variable).identifier == (simplifiedOperator.right as Variable).identifier && (simplifiedOperator.left as Variable).exponent.CompareTo((simplifiedOperator.right as Variable).exponent)))
            {
                res = NotNumberOperation(simplifiedOperator.left as Variable, simplifiedOperator.right as Variable);
            }
            else
            {
                res = simplifiedOperator;
            }

            if (res is Operator)
            {
                res = CurrectOperator(res as Operator);
            }

            if (!((evaluatedRes = res.Evaluate()) is Error))
            {
                res = evaluatedRes;
            }

            return res;
        }

        private Expression SimplifyMultiAdd(Expression other)
        {
            Expression res = null;

            if (other is Number)
            {
                if (left is Number)
                {
                    res = new Add(left + other, right);
                }
                else if (right is Number)
                {
                    res = new Add(left, right + other);
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
            else if (other is Variable)
            {
                if (left is Variable && ((left as Variable).identifier == (other as Variable).identifier && (left as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Add(NotNumberOperation(left as Variable, other as Variable), right);
                }
                else if (right is Variable && ((right as Variable).identifier == (other as Variable).identifier && (right as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Add(left, NotNumberOperation(right as Variable, other as Variable));
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

        private Expression NotNumberOperation(Variable left, Variable right)
        {
            var res = left.Clone();

            (res as Variable).prefix = (left.prefix + right.prefix) as Number;

            res.parent = parent;
            return res;
        }

        private Operator CurrectOperator(Operator res)
        {
            if (res.right is Number && (res.right as Number).IsNegative())
            {
                (res.right as Number).ToNegative();
                return new Sub(res.left, res.right);
            }
            else if (res.right is Variable && (res.right as Variable).prefix.IsNegative())
            {
                (res.right as Variable).prefix.ToNegative();
                return new Sub(res.left, res.right);
            }

            return res;
        }

        public override Expression Clone()
        {
            return new Add(left.Clone(), right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Sub(other, right);
        }

        public Operator Swap()
        {
            return new Add(right, left);
        }
    }

    public class Sub : Operator, ISwappable, IInvertable
    {
        public Sub() : base("-", 20) { }
        public Sub(Expression left, Expression right) : base(left, right, "-", 20) { }

        public override Expression Evaluate ()
        {
            return left - right;
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

            return res;
        }

        public override Expression Clone()
        {
            return new Sub(left.Clone(), right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Add(other, right);
        }

        public Operator Swap()
        {
            return new Add(new Mul(new Integer(-1), right).Simplify(), left);
        }
    }

    public class Mul : Operator, ISwappable, IInvertable
    {
        public Mul() : base("*", 30) { }
        public Mul(Expression left, Expression right) : base(left, right, "*", 30) { }

        public override Expression Evaluate ()
        {
            return left * right;
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
            Expression evaluatedRes, res = null;
            Operator simplifiedOperator = new Mul(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if (simplifiedOperator.left is Number)
            {
                if (simplifiedOperator.left.CompareTo(new Integer(0)))
                {
                    res = new Integer(0);
                }
                else if (simplifiedOperator.left.CompareTo(new Integer(1)))
                {
                    res = simplifiedOperator.right;
                }
                else if (simplifiedOperator.right is Variable)
                {
                    res = simplifiedOperator.right;
                    (res as Variable).prefix = ((res as Variable).prefix * simplifiedOperator.left) as Number;
                }
                else
                {
                    res = simplifiedOperator;
                }
            }
            else if (simplifiedOperator.right is Number)
            {
                if (simplifiedOperator.right.CompareTo(new Integer(0)))
                {
                    res = new Integer(0);
                }
                else if (simplifiedOperator.right.CompareTo(new Integer(1)))
                {
                    res = simplifiedOperator.left;
                }
                else if (simplifiedOperator.left is Variable)
                {
                    res = (simplifiedOperator.left as Variable).Clone();
                    (res as Variable).prefix = ((res as Variable).prefix * simplifiedOperator.right) as Number;
                }
                else
                {
                    res = simplifiedOperator;
                }
            }
            else if (simplifiedOperator.left is Mul)
            {
                res = (simplifiedOperator.left as Mul).SimplifyMultiMul(simplifiedOperator.right);
            }
            else if (simplifiedOperator.right is Mul)
            {
                res = (simplifiedOperator.right as Mul).SimplifyMultiMul(simplifiedOperator.left);
            }
            else if (simplifiedOperator.left is Div)
            {
                res = new Div(new Mul((simplifiedOperator.left as Div).left, simplifiedOperator.right), (simplifiedOperator.left as Div).right);
            }
            else if (simplifiedOperator.right is Div)
            {
                res = new Div(new Mul((simplifiedOperator.right as Div).left, simplifiedOperator.left), (simplifiedOperator.right as Div).right);
            }
            else if ((simplifiedOperator.left is Exp && simplifiedOperator.right is Exp) && (simplifiedOperator.left as Exp).right.CompareTo((simplifiedOperator.right as Exp).right))
            {
                res = new Exp(new Mul((simplifiedOperator.left as Exp).left, (simplifiedOperator.right as Exp).left), (simplifiedOperator.left as Exp).right);
            }
            else if (simplifiedOperator.left is Variable && simplifiedOperator.right is Variable)
            {
                if (((simplifiedOperator.left as Variable).identifier == (simplifiedOperator.right as Variable).identifier && simplifiedOperator.left.GetType() == simplifiedOperator.right.GetType()))
                {
                    res = SameNotNumbersOperation(simplifiedOperator.left as Variable, simplifiedOperator.right as Variable);
                }
                else if (!(simplifiedOperator.left as Variable).exponent.CompareTo(new Integer(1)) && (simplifiedOperator.left as Variable).exponent.CompareTo((simplifiedOperator.right as Variable).exponent))
                {
                    res = DifferentNotNumbersOperation(simplifiedOperator.left as Variable, simplifiedOperator.right as Variable);
                }
                else
                {
                    res = simplifiedOperator;
                }
            }
            else
            {
                res = simplifiedOperator;
            }

            if (!((evaluatedRes = res.Evaluate()) is Error))
            {
                res = evaluatedRes;
            }

            return res;
        }

        private Expression SimplifyMultiMul(Expression other)
        {
            Expression res = null;

            if (other is Number)
            {
                if (other.CompareTo(new Integer(0)))
                {
                    res = new Integer(0);
                }
                else if (other.CompareTo(new Integer(1)))
                {
                    res = this;
                }
                else if (left is Number)
                {
                    res = new Mul(left * other, right);
                }
                else if (right is Number)
                {
                    res = new Mul(left, right * other);
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
            else if (other is Variable)
            {
                if (left is Variable && ((left as Variable).identifier == (other as Variable).identifier && left.GetType() == other.GetType()))
                {
                    res = new Mul(SameNotNumbersOperation(left as Variable, other as Variable), right);
                }
                else if (right is Variable && ((right as Variable).identifier == (other as Variable).identifier && right.GetType() == other.GetType()))
                {
                    res = new Mul(left, SameNotNumbersOperation(right as Variable, other as Variable));
                }
                if (left is Variable && (!(left as Variable).exponent.CompareTo(new Integer(1)) && (left as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Mul(DifferentNotNumbersOperation(left as Variable, other as Variable), right);
                }
                else if (right is Variable && (!(right as Variable).exponent.CompareTo(new Integer(1)) && (right as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Mul(left, DifferentNotNumbersOperation(right as Variable, other as Variable));
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
            else if (other is Exp)
            {
                if (left is Exp && (left as Exp).right.CompareTo((other as Exp).right))
                {
                    res = new Mul(right, new Exp(new Mul((left as Exp).left, (other as Exp).left), (left as Exp).right));
                }
                else if (right is Exp && (right as Exp).right.CompareTo((other as Exp).right))
                {
                    res = new Mul(left, new Exp(new Mul((right as Exp).left, (other as Exp).left), (right as Exp).right));
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

        private Expression SameNotNumbersOperation(Variable left, Variable right)
        {
            Expression res;

            res = left.Clone();
            (res as Variable).prefix = (left.prefix * right.prefix) as Number;
            (res as Variable).exponent = (left.exponent + right.exponent) as Number;

            return res;
        }

        private Expression DifferentNotNumbersOperation(Variable left, Variable right)
        {
            var newLeft = left.Clone();
            var newRight = right.Clone();
            (newLeft as Variable).exponent = new Integer(1);
            (newRight as Variable).exponent = new Integer(1);

            return new Mul(left.prefix * right.prefix, new Exp(new Mul(newLeft, newRight), left.exponent));
        }

        public override Expression Clone()
        {
            return new Mul(left.Clone(), right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Div(other, right);
        }

        public Operator Swap()
        {
            return new Mul(right, left);
        }
    }

    public class Div : Operator, IInvertable
    {
        public Div() : base("/", 35) { }
        public Div(Expression left, Expression right) : base(left, right, "/", 35) { }

        public override Expression Evaluate()
        {
            return left / right;
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
            Expression evaluatedRes, res = null;
            Operator simplifiedOperator = new Div(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if (simplifiedOperator.left is Div)
            {
                res = new Div((simplifiedOperator.left as Div).left, new Mul((simplifiedOperator.left as Div).right, simplifiedOperator.right));
            }
            else if (simplifiedOperator.right is Div)
            {
                res = new Div(new Mul(simplifiedOperator.left, (simplifiedOperator.right as Div).right), (simplifiedOperator.right as Div).left);
            }
            else if ((simplifiedOperator.left is Exp && simplifiedOperator.right is Exp) && (simplifiedOperator.left as Exp).left.CompareTo((simplifiedOperator.right as Exp).left))
            {
                res = new Exp((simplifiedOperator.left as Exp).left, new Sub((simplifiedOperator.left as Exp).right, (simplifiedOperator.right as Exp).right));
            }
            else if (simplifiedOperator.left is Variable && simplifiedOperator.right is Variable && (simplifiedOperator.left as Variable).identifier == (simplifiedOperator.right as Variable).identifier)
            {
                res = NotNumberOperation(simplifiedOperator.left as Variable, simplifiedOperator.right as Variable);
            }
            else
            {
                res = simplifiedOperator;
            }

            if (!((evaluatedRes = res.Evaluate()) is Error))
            {
                res = evaluatedRes;
            }

            return res;
        }

        private Expression NotNumberOperation(Variable left, Variable right)
        {
            Expression res;

            if (((left.exponent < right.exponent) as Boolean).value)
            {
                var symbol = right.Clone();

                (symbol as Variable).exponent = (right.exponent - left.exponent) as Number;
                res = new Div(left.prefix, symbol);
            }
            else if (((left.exponent > right.exponent) as Boolean).value)
            {
                var symbol = right.Clone();

                (symbol as Variable).exponent = (left.exponent - right.exponent) as Number;
                res = new Div(symbol, right.prefix);
            }
            else
            {
                res = left.prefix / right.prefix;
            }

            return res;
        }

        public override Expression Clone()
        {
            return new Div(left.Clone(), right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Mul(other, right);
        }
    }

    public class Exp : Operator, IInvertable
    {
        public Exp() : base("^", 40) { }
        public Exp(Expression left, Expression right) : base(left, right, "^", 40) { }

        public override Expression Evaluate()
        {
            return left ^ right;
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

        public override Expression Simplify()
        {
            Expression evaluatedLeft, evaluatedRight, res = null;
            Operator simplifiedOperator = new Exp(Evaluator.SimplifyExp(left), Evaluator.SimplifyExp(right));

            if (simplifiedOperator.left is Number && simplifiedOperator.left.CompareTo(new Integer(1)))
            {
                return new Integer(1);
            }
            else if (simplifiedOperator.right is Number && simplifiedOperator.left.CompareTo(new Integer(0)))
            {
                return new Integer(1);
            }
            else if (simplifiedOperator.left is Variable && simplifiedOperator.right is Number)
            {
                res = NotNumberOperation(simplifiedOperator.left as Variable, simplifiedOperator.right as Number);
            }
            else
            {
                res = simplifiedOperator;
            }

            if (res is Exp && !((evaluatedLeft = (res as Exp).left.Evaluate()) is Error))
            {
                (res as Exp).left = evaluatedLeft;
            }

            if (res is Exp && !((evaluatedRight = (res as Exp).right.Evaluate()) is Error))
            {
                (res as Exp).right = evaluatedRight;
            }

            return res;
        }

        private Variable NotNumberOperation(Variable left, Number right)
        {
            Variable res = left.Clone() as Variable;

            res.exponent = (left.exponent * right) as Number;

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

        public override Expression Clone()
        {
            return new Exp(left.Clone(), right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            throw new NotImplementedException();
        }
    }
}