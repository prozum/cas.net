using System;
using System.Collections.Generic;

namespace Ast
{
    public interface ISwappable
    {
        Operator Swap();
        Operator Transform();
    }

    public abstract class Operator : Expression
    {
        public string symbol;
        public int priority;

        private Expression left;
        public Expression Left
        {
            get
            {
                return left;
            }
            set
            {
                if (value != null)
                value.parent = this;

                left = value;
            }
        }

        private Expression right;
        public Expression Right
        {
            get
            {
                return right;
            }
            set
            {
                if (value != null)
                    value.parent = this;

                right = value;
            }
        }

        public Operator(string symbol, int priority) : this(null, null, symbol, priority) { }
        public Operator(Expression left, Expression right, string symbol, int priority)
        {
            Left = left;
            Right = right;
            this.symbol = symbol;
            this.priority = priority;
        }

        public override Expression Evaluate()
        {
            return new Error(this, "Cannot evaluate operator expression!");
        }

        public override string ToString()
        {
            if (parent == null || priority >= parent.priority)
            {
                return Left.ToString () + symbol + Right.ToString ();
            } 
            else 
            {
                return '(' + Left.ToString () + symbol + Right.ToString () + ')';
            }
        }

        public override bool CompareTo(Expression other)
        {
            Expression thisSimplified = Simplify();
            Expression otherSimplified = other.Simplify();

            if (!(thisSimplified is Operator))
            {
                return thisSimplified.CompareTo(otherSimplified);
            }
            else if (thisSimplified is Operator && otherSimplified is Operator)
            {
                if (thisSimplified is ISwappable)
                {
                    return CompareSwappables(thisSimplified as ISwappable, otherSimplified as Operator);
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

        private bool CompareSwappables(ISwappable exp1, Operator exp2)
        {
            if (CompareSides(exp1 as Operator, exp2) || CompareSides(exp1.Swap().Simplify() as Operator, exp2))
            {
                return true;
            }
            else if ((exp1 as Operator).Left is ISwappable || (exp1 as Operator).Right is ISwappable)
            {
                return SwappableCompareAlorithem(exp1, exp2);
            }

            return false;
        }

        private bool SwappableCompareAlorithem(ISwappable exp1, Operator exp2)
        {
            Operator modified = (exp1 as Operator).Clone() as Operator;

            do
            {
                modified = (modified as ISwappable).Transform();

                if (modified.CompareTo(exp2))
                {
                    return true;
                }

                if (modified.Left is ISwappable)
                {
                    modified.Left = (modified.Left as ISwappable).Swap();

                    if (modified.CompareTo(exp2))
                    {
                        return true;
                    }
                }
                else if (modified.Right is ISwappable)
                {
                    modified.Right = (modified.Right as ISwappable).Swap();

                    if (modified.CompareTo(exp2))
                    {
                        return true;
                    }
                }
            } while (!modified.CompareTo(exp1 as Operator));

            return false;
        }

        private bool CompareSides(Operator exp1, Operator exp2)
        {
            return exp1.Left.CompareTo(exp2.Left) && exp1.Right.CompareTo(exp2.Right);
        }

        public override bool ContainsVariable(Variable other)
        {
            return Left.ContainsVariable(other) || Right.ContainsVariable(other);
        }

        public override Expression Simplify()
        {
            var prev = ToString();
            var res = SimplifyHelper(Left.Simplify(), Right.Simplify());

            if (prev != res.ToString())
            {
                res = res.Simplify();
            }

            return res;
        }

        protected virtual Expression SimplifyHelper(Expression left, Expression right)
        {
            return this;
        }

        public override Expression Expand()
        {
            var prev = ToString();
            var res = ExpandHelper(Left.Expand(), Right.Expand());

            if (prev != res.ToString())
            {
                res = res.Expand();
            }

            return res;
        }

        protected virtual Expression ExpandHelper(Expression left, Expression right)
        {
            return this;
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

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            return new Equal(Left.Simplify(), Right.Simplify());
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Equal(Left.Expand(), Right.Expand());
        }

        public override Expression Clone()
        {
            return new Equal(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new Equal(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }

    public class Assign : Operator
    {
        public Assign() : base(":=", 0) { }
        public Assign(Expression left, Expression right) : base(left, right, ":=", 0) { }

<<<<<<< HEAD
        public override Expression Evaluate()
        {

            if (Left is Symbol)
            {
                var sym = (Symbol)Left;
                sym.scope.SetVar(sym.identifier, Right);
                return new Info(sym.identifier + " assigned");
            }

            if (Left is InstanceFunc)
            {
                var insFunc = (InstanceFunc)Left;

                foreach (var arg in insFunc.args)
                {
                    if (!(arg is Symbol))
                        return new Error(this, "All arguments must be symbols");
                }

                var defFunc = new UsrFunc(insFunc.identifier, insFunc.args, insFunc.scope);

                defFunc.expr = Right;

                insFunc.scope.SetVar(insFunc.identifier, defFunc);

                return new Info(insFunc.ToString() + " assigned");
            }

            return new Error(this, "Left operand must be Symbol or Function");
        }

        public override Expression Expand()
        {
=======
        protected override Expression ExpandHelper(Expression left, Expression right)
        {
>>>>>>> 963d3e36dbd286a1e62bb6b93f55acebb4bcd975
            return new Assign(Left.Expand(), Right.Expand());
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            return new Assign(Left.Simplify(), Right.Simplify());
        }

        public override Expression Clone()
        {
            return new Assign(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new Assign(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }

    public class Add : Operator, ISwappable, IInvertable
    {
        public Add() : base("+", 20) { }
        public Add(Expression left, Expression right) : base(left, right, "+", 20) { }

        public override Expression Evaluate ()
        {
            return Left + Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Add(left.Expand(), right.Expand());
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            if (left is Number && left.CompareTo(Constant.Zero))
            {
                return right;
            }
            else if (right is Number && right.CompareTo(Constant.Zero))
            {
                return left;
            }
            else if (left is Number && right is Number)
            {
                return left + right;
            }
            else if (left is Add)
            {
                return (left as Add).SimplifyMultiAdd(right);
            }
            else if (right is Add)
            {
                return (right as Add).SimplifyMultiAdd(left);
            }
            else if ((left is Variable && right is Variable) && CompareVariables(left as Variable, right as Variable))
            {
                return VariableOperation(left as Variable, right as Variable);
            }
            else if (left.CompareTo(Right))
            {
                return new Mul(new Integer(2), left);
            }
            else
            {
                return new Add(left, right);
            }
        }

        private bool CompareVariables(Variable left, Variable right)
        {
            return left.identifier == right.identifier && left.exponent.CompareTo(right.exponent) && left.GetType() == right.GetType();
        }

        private Expression SimplifyMultiAdd(dynamic other)
        {
            if (other is Variable || other is Number)
            {
                return SimplifyMultiAdd(other);
            }
            else
            {
                if (Left.CompareTo(other))
                {
                    return new Add(new Mul(new Integer(2), other).Simplify(), Right);
                }
                else if (Right.CompareTo(other))
                {
                    return new Add(Left, new Mul(new Integer(2), other).Simplify());
                }
                else
                {
                    return new Add(this, other);
                }
            }
        }

        private Expression SimplifyMultiAdd(Number other)
            {
                if (Left is Number)
                {
                return new Add(Left + other, Right);
                }
                else if (Right is Number)
                {
                return new Add(Left, Right + other);
                }
                else
                {
                return new Add(this, other);
            }
                }

        private Expression SimplifyMultiAdd(Variable other)
            {
            if (Left is Variable && CompareVariables(Left as Variable, other))
                {
                return new Add(VariableOperation(Left as Variable, other), Right);
                }
            else if (Right is Variable && CompareVariables(Right as Variable, other))
                {
                return new Add(Left, VariableOperation(Right as Variable, other));
                }
                else if (Left is Add)
                {
                var res = new Add((Left as Add).SimplifyMultiAdd(other), Right);

                    if (res.ToString() == new Add(new Add(Left, other), Right).ToString())
                    {
                        res = new Add(this, other);
                    }

                return res;
                }
                else if (Right is Add)
                {
                var res = new Add(Left, (Right as Add).SimplifyMultiAdd(other));

                    if (res.ToString() == new Add(Left, new Add(Right, other)).ToString())
                    {
                        res = new Add(this, other);
                }

                return res;
                }
                else
                {
                return new Add(this, other);
            }
        }

        private Expression VariableOperation(Variable left, Variable right)
        {
            var res = left.Clone();

            (res as Variable).prefix = (left.prefix + right.prefix) as Number;

            return res;
        }

        public override Expression CurrectOperator()
        {
            if (Right is Number && (Right as Number).IsNegative())
            {
                return new Sub(Left.CurrectOperator(), (Right as Number).ToNegative());
            }
            else if (Right is Variable && (Right as Variable).prefix.IsNegative())
            {
                var newRight = Right.Clone();
                (newRight as Symbol).prefix = (newRight as Symbol).prefix.ToNegative();

                return new Sub(Left.CurrectOperator(), newRight);
            }

            return new Add(Left.CurrectOperator(), Right.CurrectOperator());
        }

        public override Expression Clone()
        {
            return new Add(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Sub(other, Right);
        }

        public Operator Swap()
        {
            return new Add(Right, Left);
        }

        public Operator Transform()
        {
            if (Left is Add)
            {
                return new Add((Left as Add).Left, new Add((Left as Add).Right, Right));
            }
            else if (Right is Add)
            {
                return new Add(new Add(Left, (Right as Add).Left), (Right as Add).Right);
            }
            else if (Left is Sub)
            {
                return new Sub((Left as Sub).Left, new Add((Left as Sub).Right, Right));
            }
            else if (Right is Sub)
            {
                return new Sub(new Add(Left, (Right as Sub).Left), (Right as Sub).Right);
            }
            else
            {
                return this;
            }
        }

        public override string ToString()
        {
            var sym = symbol;
            var tempRight = Right;

            if (Right is Number && (Right as Number).IsNegative())
            {
                tempRight = (Right as Number).ToNegative();
                sym = "-";
            }

            if (parent == null || priority >= parent.priority)
            {
                return Left.ToString() + sym + tempRight.ToString();
            }
            else
            {
                return '(' + Left.ToString() + sym + tempRight.ToString() + ')';
            }
        }
    }

    public class Sub : Operator, ISwappable, IInvertable
    {
        public Sub() : base("-", 20) { }
        public Sub(Expression left, Expression right) : base(left, right, "-", 20) { }

        public override Expression Evaluate ()
        {
            return Left - Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Sub(left.Expand(), right.Expand());
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            var newRight = new Mul(new Integer(-1), right).Simplify();
            return new Add(left, newRight);
        }

        public override Expression Clone()
        {
            return new Sub(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Add(other, Right);
        }

        public Operator Swap()
        {
            return new Add(new Mul(new Integer(-1), Right), Left);
        }

        public Operator Transform()
        {
            if (Left is Add)
            {
                return new Add((Left as Add).Left, new Sub((Left as Add).Right, Right));
            }
            else if (Right is Add)
            {
                return new Add(new Sub(Left, (Right as Add).Left), (Right as Add).Right);
            }
            else if (Left is Sub)
            {
                return new Sub((Left as Sub).Left, new Sub((Left as Sub).Right, Right));
            }
            else if (Right is Sub)
            {
                return new Sub(new Sub(Left, (Right as Sub).Left), (Right as Sub).Right);
            }
            else
            {
                return this;
            }
        }

        public override Expression CurrectOperator()
        {
            if (Right is Number && (Right as Number).IsNegative())
            {
                return new Add(Left.CurrectOperator(), (Right as Number).ToNegative());
            }
            else if (Right is Variable && (Right as Variable).prefix.IsNegative())
            {
                var newRight = Right.Clone();
                (newRight as Symbol).prefix = (newRight as Symbol).prefix.ToNegative();

                return new Add(Left.CurrectOperator(), newRight);
            }

            return new Sub(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }

    public class Mul : Operator, ISwappable, IInvertable
    {
        public Mul() : base("*", 30) { }
        public Mul(Expression left, Expression right) : base(left, right, "*", 30) { }

        public override Expression Evaluate ()
        {
            return Left * Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    return new Add(new Mul((left as Operator).Left, right), new Mul((left as Operator).Right, right));
                }
                else if (left is Sub)
                {
                    return new Sub(new Mul((left as Operator).Left, right), new Mul((left as Operator).Right, right));
                }
                else
                {
                    return new Mul(left.Expand(), right.Expand());
                }
            }
            else if (right is Operator && (right as Operator).priority < priority)
            {
                if (right is Add)
                {
                    return new Add(new Mul((right as Operator).Left, left), new Mul((right as Operator).Right, left));
                }
                else if (right is Sub)
                {
                    return new Sub(new Mul((right as Operator).Left, left), new Mul((right as Operator).Right, left));
                }
                else
                {
                    return new Mul(left.Expand(), right.Expand());
                }
            }
            else
            {
                return new Mul(left.Expand(), right.Expand());
            }
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            if (left is Number)
            {
                if (left.CompareTo(Constant.Zero))
                {
                    return new Integer(0);
                }
                else if (left.CompareTo(Constant.One))
                {
                    return right;
                }
                else if (right is Variable)
                {
                    var res = right.Clone();
                    (res as Variable).prefix = ((res as Variable).prefix * left) as Number;
                    return res;
                }
                else if (right is Number)
                {
                    return left * right;
                }
            }
            else if (right is Number)
            {
                if (right.CompareTo(Constant.Zero))
                {
                    return new Integer(0);
                }
                else if (right.CompareTo(Constant.One))
                {
                    return left;
                }
                else if (left is Variable)
                {
                    var res = left.Clone();
                    (res as Variable).prefix = ((res as Variable).prefix * right) as Number;
                    return res;
                }
                else if (left is Number)
                {
                    return left * right;
                }
            }
            else if (left is Mul)
            {
                return (left as Mul).SimplifyMultiMul(right);
            }
            else if (right is Mul)
            {
                return (right as Mul).SimplifyMultiMul(left);
            }
            else if (left is Div)
            {
                return new Div(new Mul((left as Div).Left, right), (left as Div).Right);
            }
            else if (right is Div)
            {
                return new Div(new Mul((right as Div).Left, left), (right as Div).Right);
            }
            else if ((left is Exp && right is Exp) && (left as Exp).Right.CompareTo((right as Exp).Right))
            {
                return new Exp(new Mul((left as Exp).Left, (right as Exp).Left), (left as Exp).Right);
            }
            else if (left is Variable && right is Variable)
            {
                if (CompareVariables(left as Variable, right as Variable))
                {
                    return SameVariableOperation(left as Variable, right as Variable);
                }
                else if (!(left as Variable).exponent.CompareTo(Constant.One) && (left as Variable).exponent.CompareTo((right as Variable).exponent))
                {
                    return DifferentVariableOperation(left as Variable, right as Variable);
                }
            }

            return new Mul(left, right);
        }

        private bool CompareVariables(Variable left, Variable right)
        {
            return left.identifier == right.identifier && left.GetType() == right.GetType();
        }

        private Expression SimplifyMultiMul(dynamic other)
        {
            if (other is Variable || other is Number)
            {
                return SimplifyMultiMul(other);
            }
            else
            {
                if (Left.CompareTo(other))
                {
                    return new Mul(new Exp(other, new Integer(2)).Simplify(), Right);
                }
                else if (Right.CompareTo(other))
                {
                    return new Mul(Left, new Exp(other, new Integer(2)).Simplify());
                }
                else
                {
                    return new Mul(this, other);
                }
            }
        }

        private Expression SimplifyMultiMul(Number other)
            {
                if (other.CompareTo(Constant.Zero))
                {
                return new Integer(0);
                }
                else if (other.CompareTo(Constant.One))
                {
                return this;
                }
                else if (Left is Number)
                {
                return new Mul(Left * other, Right);
                }
                else if (Right is Number)
                {
                return new Mul(Left, Right * other);
                }
                else
                {
                return new Mul(this, other);
            }
                }

        private Expression SimplifyMultiMul(Variable other)
            {
            if (Left is Variable && CompareVariables(Left as Variable, other))
                {
                return new Mul(SameVariableOperation(Left as Variable, other), Right);
                }
            else if (Right is Variable && CompareVariables(Right as Variable, other))
                {
                return new Mul(Left, SameVariableOperation(Right as Variable, other));
                }
                if (Left is Variable && (!(Left as Variable).exponent.CompareTo(Constant.One) && (Left as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                return new Mul(DifferentVariableOperation(Left as Variable, other as Variable), Right);
                }
                else if (Right is Variable && (!(Right as Variable).exponent.CompareTo(Constant.One) && (Right as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                return new Mul(Left, DifferentVariableOperation(Right as Variable, other as Variable));
                }
                else if (Left is Mul)
                {
                var res = new Mul((Left as Mul).SimplifyMultiMul(other), Right);

                    if (res.ToString() == new Mul(new Mul(Left, other), Right).ToString())
                    {
                        res = new Mul(this, other);
                    }

                return res;
                }
                else if (Right is Mul)
                {
                var res = new Mul(Left, (Right as Mul).SimplifyMultiMul(other));

                    if (res.ToString() == new Mul(Left, new Mul(Right, other)).ToString())
                    {
                        res = new Mul(this, other);
                    }

                return res;
                }
                else
                {
                return new Mul(this, other);
                }
            }

        private Expression SameVariableOperation(Variable left, Variable right)
        {
            var res = left.Clone();

            (res as Variable).prefix = (left.prefix * right.prefix) as Number;
            (res as Variable).exponent = (left.exponent + right.exponent) as Number;

            return res;
        }

        private Expression DifferentVariableOperation(Variable left, Variable right)
        {
            var newLeft = left.Clone();
            var newRight = right.Clone();
            (newLeft as Variable).exponent = new Integer(1);
            (newRight as Variable).exponent = new Integer(1);

            return new Mul(left.prefix * right.prefix, new Exp(new Mul(newLeft, newRight), left.exponent));
        }

        public override Expression Clone()
        {
            return new Mul(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Div(other, Right);
        }

        public override Expression CurrectOperator()
        {
            return new Mul(Left.CurrectOperator(), Right.CurrectOperator());
        }

        public Operator Swap()
        {
            return new Mul(Right, Left);
        }

        public Operator Transform()
        {
            if (Left is Mul)
            {
                return new Mul((Left as Mul).Left, new Mul((Left as Mul).Right, Right));
            }
            else if (Right is Mul)
            {
                return new Mul(new Mul(Left, (Right as Mul).Left), (Right as Mul).Right);
            }
            else
            {
                return this;
            }
        }
    }

    public class Div : Operator, IInvertable
    {
        public Div() : base("/", 40) { }
        public Div(Expression left, Expression right) : base(left, right, "/", 40) { }

        public override Expression Evaluate()
        {
            return Left / Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    return new Add(new Div((left as Operator).Left, right), new Div((left as Operator).Right, right));
                }
                else if (left is Sub)
                {
                    return new Sub(new Div((left as Operator).Left, right), new Div((left as Operator).Right, right));
                }
                else
                {
                    return new Div(left.Expand(), right.Expand());
                }
            }
            else if (right is Operator && (right as Operator).priority < priority)
            {
                if (right is Add)
                {
                    return new Add(new Div((right as Operator).Left, left), new Div((right as Operator).Right, Left));
                }
                else if (right is Sub)
                {
                    return new Sub(new Div((right as Operator).Left, left), new Div((right as Operator).Right, Left));
                }
                else
                {
                    return new Div(left.Expand(), right.Expand());
                }
            }
            else
            {
                return new Div(left.Expand(), right.Expand());
            }
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            if (right is Number && right.CompareTo(Constant.One))
            {
                return left;
            }
            else if (left is Number && right is Number)
            {
                return left / right;
            }
            else if (left is Div)
            {
                return new Div((left as Div).Left, new Mul((left as Div).Right, right));
            }
            else if (right is Div)
            {
                return new Div(new Mul(left, (right as Div).Right), (right as Div).Left);
            }
            else if ((left is Exp && right is Exp) && (left as Exp).Left.CompareTo((right as Exp).Left))
            {
                return new Exp((left as Exp).Left, new Sub((left as Exp).Right, (right as Exp).Right));
            }
            else if (left is Variable && right is Variable && CompareVariables(left as Variable, right as Variable))
            {
                return VariableOperation(left as Variable, right as Variable);
            }

            return new Div(left, right);
        }

        private bool CompareVariables(Variable left, Variable right)
        {
            return left.identifier == right.identifier && left.GetType() == right.GetType();
        }

        private Expression VariableOperation(Variable left, Variable right)
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
            return new Div(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Mul(other, Right);
        }

        public override Expression CurrectOperator()
        {
            return new Div(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }

    public class Exp : Operator, IInvertable
    {
        public Exp() : base("^", 50) { }
        public Exp(Expression left, Expression right) : base(left, right, "^", 40) { }

        public override Expression Evaluate()
        {
            return Left ^ Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    return new Add(new Add(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right)), new Mul(new Integer(2), new Mul((left as Operator).Left, (left as Operator).Right)));
                }
                else if (left is Sub)
                {
                    return new Sub(new Add(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right)), new Mul(new Integer(2), new Mul((left as Operator).Left, (left as Operator).Right)));
                }
                else if (left is Mul)
                {
                    return new Mul(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right));
                }
                else if (left is Div)
                {
                    return new Div(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right));
                }
                else
                {
                    return new Exp(left.Expand(), right.Expand());
                }
            }
            else
            {
                return new Exp(left.Expand(), right.Expand());
            }
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            if (left is Number && left.CompareTo(Constant.One))
            {
                return new Integer(1);
            }
            else if (right is Number && left.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }
            else if (left is Number && right is Number)
            {
                return left ^ right;
            }
            else if (left is Variable && right is Number)
            {
                return VariableOperation(left as Variable, right as Number).Simplify();
            }

            return new Exp(left, right);
        }

        private Variable VariableOperation(Variable left, Number right)
        {
            Variable res = left.Clone() as Variable;

            res.exponent = (left.exponent * right) as Number;

            return res;
        }

        public override Expression Clone()
        {
            return new Exp(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            throw new NotImplementedException();
        }

        public override Expression CurrectOperator()
        {
            return new Exp(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}