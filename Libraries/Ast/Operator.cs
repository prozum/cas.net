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
        public Expression Left
        {
            get
            {
                return _left;
            }
            set
            {
                if (value != null)
                    value.parent = this;
                _left = value;
            }
        }
        private Expression _left;
        public Expression Right
        {
            get
            {
                return _right;
            }
            set
            {
                if (value != null)
                    value.parent = this;
                _right = value;
            }
        }
        private Expression _right;

        public Operator(string symbol, int priority)
        {
            this.Left = null;
            this.Right = null;
            this.symbol = symbol;
            this.priority = priority;
        }
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

//        public override void SetFunctionCall(UserDefinedFunction functionCall)
//        {
//            Left.SetFunctionCall(functionCall);
//            Right.SetFunctionCall(functionCall);
//        }

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
                        return CompareSides(thisSimplified as Operator, otherSimplified as Operator) || CompareSides((thisSimplified as ISwappable).Swap().Simplify() as Operator, otherSimplified as Operator);
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
            return exp1.Left.CompareTo(exp2.Left) && exp1.Right.CompareTo(exp2.Right);
        }

        public override bool ContainsVariable(Variable other)
        {
            return Left.ContainsVariable(other) || Right.ContainsVariable(other);
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
            var res = new Equal(Left.Expand(), Right.Expand());

            return res;
        }

        public override Expression Simplify()
        {
            Expression res = null, evaluatedLeft, evaluatedRight;

            res = new Equal(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));

            if (!((evaluatedLeft = (res as Equal).Left.Evaluate()) is Error))
            {
                (res as Equal).Left = evaluatedLeft;
            }

            if (!((evaluatedRight = (res as Equal).Right.Evaluate()) is Error))
            {
                (res as Equal).Right = evaluatedRight;
            }

            return res;
        }

        public override Expression Clone()
        {
            return new Equal(Left.Clone(), Right.Clone());
        }
    }

    public class Assign : Operator
    {
        public Assign() : base(":=", 0) { }
        public Assign(Expression left, Expression right) : base(left, right, ":=", 0) { }

        public override Expression Evaluate()
        {

            if (Left is Symbol)
            {
                var sym = (Symbol)Left;
                sym.scope.SetVar(sym.identifier, Right);
                return new Info(sym.identifier + " assigned");
            }

            if (Left is UsrFunc)
            {
                var func = (UsrFunc)Left;
                func.scope.SetVar(func.identifier, Right);

                return new Info(func.ToString() + " assigned");
            }

            return new Error(this, "Left operand must be Symbol or Function");
        }

        public override Expression Expand()
        {
            return new Assign(Left.Expand(), Right.Expand());
        }

        public override Expression Simplify()
        {
            Expression res = null, evaluatedLeft, evaluatedRight;

            res = new Assign(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));

            if (!((evaluatedLeft = (res as Assign).Left.Evaluate()) is Error))
            {
                (res as Assign).Left = evaluatedLeft;
            }

            if (!((evaluatedRight = (res as Assign).Right.Evaluate()) is Error))
            {
                (res as Assign).Right = evaluatedRight;
            }

            return res;
        }

        public override Expression Clone()
        {
            return new Assign(Left.Clone(), Right.Clone());
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

        public override Expression Expand()
        {
            var res = new Add(Left.Expand(), Right.Expand());

            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedRes, res = null;
            Operator simplifiedOperator = new Add(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));

            if (simplifiedOperator.Left is Number && simplifiedOperator.Left.CompareTo(Constant.Zero))
            {
                res = simplifiedOperator.Right;
            }
            else if (simplifiedOperator.Right is Number && simplifiedOperator.Right.CompareTo(Constant.Zero))
            {
                res = simplifiedOperator.Left;
            }
            else if (simplifiedOperator.Left is Add)
            {
                res = (simplifiedOperator.Left as Add).SimplifyMultiAdd(simplifiedOperator.Right);
            }
            else if (simplifiedOperator.Right is Add)
            {
                res = (simplifiedOperator.Right as Add).SimplifyMultiAdd(simplifiedOperator.Left);
            }
            else if (simplifiedOperator.Left is Variable && simplifiedOperator.Right is Variable && ((simplifiedOperator.Left as Variable).identifier == (simplifiedOperator.Right as Variable).identifier && (simplifiedOperator.Left as Variable).exponent.CompareTo((simplifiedOperator.Right as Variable).exponent)))
            {
                res = NotNumberOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
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
                if (Left is Number)
                {
                    res = new Add(Left + other, Right);
                }
                else if (Right is Number)
                {
                    res = new Add(Left, Right + other);
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
                if (Left is Variable && ((Left as Variable).identifier == (other as Variable).identifier && (Left as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Add(NotNumberOperation(Left as Variable, other as Variable), Right);
                }
                else if (Right is Variable && ((Right as Variable).identifier == (other as Variable).identifier && (Right as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Add(Left, NotNumberOperation(Right as Variable, other as Variable));
                }
                else if (Left is Add)
                {
                    res = new Add((Left as Add).SimplifyMultiAdd(other), Right);

                    if (res.ToString() == new Add(new Add(Left, other), Right).ToString())
                    {
                        res = new Add(this, other);
                    }
                }
                else if (Right is Add)
                {
                    res = new Add(Left, (Right as Add).SimplifyMultiAdd(other));

                    if (res.ToString() == new Add(Left, new Add(Right, other)).ToString())
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
                if (Left.CompareTo(other))
                {
                    res = new Add(Evaluator.SimplifyExp(new Mul(new Integer(2), other)), Right);
                }
                else if (Right.CompareTo(other))
                {
                    res = new Add(Left, Evaluator.SimplifyExp(new Mul(new Integer(2), other)));
                }
                else
                {
                    res = new Add(this, other);
                }
            }

            return res;
        }

        private Expression NotNumberOperation(Variable left, Variable right)
        {
            var res = left.Clone();

            (res as Variable).prefix = (left.prefix + right.prefix) as Number;

            return res;
        }

        private Operator CurrectOperator(Operator res)
        {
            if (res.Right is Number && (res.Right as Number).IsNegative())
            {
                (res.Right as Number).ToNegative();
                return new Sub(res.Left, res.Right);
            }
            else if (res.Right is Variable && (res.Right as Variable).prefix.IsNegative())
            {
                (res.Right as Variable).prefix.ToNegative();
                return new Sub(res.Left, res.Right);
            }

            return res;
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
    }

    public class Sub : Operator, ISwappable, IInvertable
    {
        public Sub() : base("-", 20) { }
        public Sub(Expression left, Expression right) : base(left, right, "-", 20) { }

        public override Expression Evaluate ()
        {
            return Left - Right;
        }

        public override Expression Expand()
        {
            var res = new Sub(Left.Expand(), Right.Expand());

            return res;
        }

        public override Expression Simplify()
        {
            Expression res = null;

            Right = Evaluator.SimplifyExp(new Mul(new Integer(-1), Right));
            res = new Add(Left, Right).Simplify();

            return res;
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
    }

    public class Mul : Operator, ISwappable, IInvertable
    {
        public Mul() : base("*", 30) { }
        public Mul(Expression left, Expression right) : base(left, right, "*", 30) { }

        public override Expression Evaluate ()
        {
            return Left * Right;
        }

        public override Expression Expand()
        {
            Expression res = null;

            if (Left is Operator && (Left as Operator).priority < priority)
            {
                if (Left is Add)
                {
                    res = new Add(new Mul((Left as Operator).Left, Right), new Mul((Left as Operator).Right, Right));
                } 
                else if (Left is Sub)
                {
                    res = new Sub(new Mul((Left as Operator).Left, Right), new Mul((Left as Operator).Right, Right));
                }
            } 
            else if (Right is Operator && (Right as Operator).priority < priority)
            {
                if (Right is Add)
                {
                    res = new Add(new Mul((Right as Operator).Left, Left), new Mul((Right as Operator).Right, Left));
                }
                else if (Right is Sub)
                {
                    res = new Sub(new Mul((Right as Operator).Left, Left), new Mul((Right as Operator).Right, Left));
                }
            }
            else
            {
                res = new Mul(Left.Expand(), Right.Expand());
            }

            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedRes, res = null;
            Operator simplifiedOperator = new Mul(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));

            if (simplifiedOperator.Left is Number)
            {
                if (simplifiedOperator.Left.CompareTo(Constant.Zero))
                {
                    res = new Integer(0);
                }
                else if (simplifiedOperator.Left.CompareTo(Constant.One))
                {
                    res = simplifiedOperator.Right;
                }
                else if (simplifiedOperator.Right is Variable)
                {
                    res = simplifiedOperator.Right;
                    (res as Variable).prefix = ((res as Variable).prefix * simplifiedOperator.Left) as Number;
                }
                else
                {
                    res = simplifiedOperator;
                }
            }
            else if (simplifiedOperator.Right is Number)
            {
                if (simplifiedOperator.Right.CompareTo(Constant.Zero))
                {
                    res = new Integer(0);
                }
                else if (simplifiedOperator.Right.CompareTo(Constant.One))
                {
                    res = simplifiedOperator.Left;
                }
                else if (simplifiedOperator.Left is Variable)
                {
                    res = (simplifiedOperator.Left as Variable).Clone();
                    (res as Variable).prefix = ((res as Variable).prefix * simplifiedOperator.Right) as Number;
                }
                else
                {
                    res = simplifiedOperator;
                }
            }
            else if (simplifiedOperator.Left is Mul)
            {
                res = (simplifiedOperator.Left as Mul).SimplifyMultiMul(simplifiedOperator.Right);
            }
            else if (simplifiedOperator.Right is Mul)
            {
                res = (simplifiedOperator.Right as Mul).SimplifyMultiMul(simplifiedOperator.Left);
            }
            else if (simplifiedOperator.Left is Div)
            {
                res = new Div(new Mul((simplifiedOperator.Left as Div).Left, simplifiedOperator.Right), (simplifiedOperator.Left as Div).Right);
            }
            else if (simplifiedOperator.Right is Div)
            {
                res = new Div(new Mul((simplifiedOperator.Right as Div).Left, simplifiedOperator.Left), (simplifiedOperator.Right as Div).Right);
            }
            else if ((simplifiedOperator.Left is Exp && simplifiedOperator.Right is Exp) && (simplifiedOperator.Left as Exp).Right.CompareTo((simplifiedOperator.Right as Exp).Right))
            {
                res = new Exp(new Mul((simplifiedOperator.Left as Exp).Left, (simplifiedOperator.Right as Exp).Left), (simplifiedOperator.Left as Exp).Right);
            }
            else if (simplifiedOperator.Left is Variable && simplifiedOperator.Right is Variable)
            {
                if (((simplifiedOperator.Left as Variable).identifier == (simplifiedOperator.Right as Variable).identifier && simplifiedOperator.Left.GetType() == simplifiedOperator.Right.GetType()))
                {
                    res = SameNotNumbersOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
                }
                else if (!(simplifiedOperator.Left as Variable).exponent.CompareTo(Constant.One) && (simplifiedOperator.Left as Variable).exponent.CompareTo((simplifiedOperator.Right as Variable).exponent))
                {
                    res = DifferentNotNumbersOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
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
                if (other.CompareTo(Constant.Zero))
                {
                    res = new Integer(0);
                }
                else if (other.CompareTo(Constant.One))
                {
                    res = this;
                }
                else if (Left is Number)
                {
                    res = new Mul(Left * other, Right);
                }
                else if (Right is Number)
                {
                    res = new Mul(Left, Right * other);
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
                if (Left is Variable && ((Left as Variable).identifier == (other as Variable).identifier && Left.GetType() == other.GetType()))
                {
                    res = new Mul(SameNotNumbersOperation(Left as Variable, other as Variable), Right);
                }
                else if (Right is Variable && ((Right as Variable).identifier == (other as Variable).identifier && Right.GetType() == other.GetType()))
                {
                    res = new Mul(Left, SameNotNumbersOperation(Right as Variable, other as Variable));
                }
                if (Left is Variable && (!(Left as Variable).exponent.CompareTo(Constant.One) && (Left as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Mul(DifferentNotNumbersOperation(Left as Variable, other as Variable), Right);
                }
                else if (Right is Variable && (!(Right as Variable).exponent.CompareTo(Constant.One) && (Right as Variable).exponent.CompareTo((other as Variable).exponent)))
                {
                    res = new Mul(Left, DifferentNotNumbersOperation(Right as Variable, other as Variable));
                }
                else if (Left is Mul)
                {
                    res = new Mul((Left as Mul).SimplifyMultiMul(other), Right);

                    if (res.ToString() == new Mul(new Mul(Left, other), Right).ToString())
                    {
                        res = new Mul(this, other);
                    }
                }
                else if (Right is Mul)
                {
                    res = new Mul(Left, (Right as Mul).SimplifyMultiMul(other));

                    if (res.ToString() == new Mul(Left, new Mul(Right, other)).ToString())
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
                if (Left is Exp && (Left as Exp).Right.CompareTo((other as Exp).Right))
                {
                    res = new Mul(Right, new Exp(new Mul((Left as Exp).Left, (other as Exp).Left), (Left as Exp).Right));
                }
                else if (Right is Exp && (Right as Exp).Right.CompareTo((other as Exp).Right))
                {
                    res = new Mul(Left, new Exp(new Mul((Right as Exp).Left, (other as Exp).Left), (Right as Exp).Right));
                }
                else
                {
                    res = new Mul(this, other);
                }
            }
            else
            {
                if (Left.CompareTo(other))
                {
                    res = new Mul(Evaluator.SimplifyExp(new Exp(other, new Integer(2))), Right);
                }
                else if (Right.CompareTo(other))
                {
                    res = new Mul(Left, Evaluator.SimplifyExp(new Mul(other, new Integer(2))));
                }
                else
                {
                    res = new Mul(this, other);
                }
            }

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
            return new Mul(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Div(other, Right);
        }

        public Operator Swap()
        {
            return new Mul(Right, Left);
        }
    }

    public class Div : Operator, IInvertable
    {
        public Div() : base("/", 35) { }
        public Div(Expression left, Expression right) : base(left, right, "/", 35) { }

        public override Expression Evaluate()
        {
            return Left / Right;
        }

        public override Expression Expand()
        {
            Expression res = null;

            if (Left is Operator && (Left as Operator).priority < priority)
            {
                if (Left is Add)
                {
                    res = new Add(new Div((Left as Operator).Left, Right), new Div((Left as Operator).Right, Right));
                }
                else if (Left is Sub)
                {
                    res = new Sub(new Div((Left as Operator).Left, Right), new Div((Left as Operator).Right, Right));
                }
            }
            else if (Right is Operator && (Right as Operator).priority < priority)
            {
                if (Right is Add)
                {
                    res = new Add(new Div((Right as Operator).Left, Left), new Div((Right as Operator).Right, Left));
                }
                else if (Right is Sub)
                {
                    res = new Sub(new Div((Right as Operator).Left, Left), new Div((Right as Operator).Right, Left));
                }
            }
            else
            {
                res = new Div(Left.Expand(), Right.Expand());
            }

            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedRes, res = null;
            Operator simplifiedOperator = new Div(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));

            if (simplifiedOperator.Left is Div)
            {
                res = new Div((simplifiedOperator.Left as Div).Left, new Mul((simplifiedOperator.Left as Div).Right, simplifiedOperator.Right));
            }
            else if (simplifiedOperator.Right is Div)
            {
                res = new Div(new Mul(simplifiedOperator.Left, (simplifiedOperator.Right as Div).Right), (simplifiedOperator.Right as Div).Left);
            }
            else if ((simplifiedOperator.Left is Exp && simplifiedOperator.Right is Exp) && (simplifiedOperator.Left as Exp).Left.CompareTo((simplifiedOperator.Right as Exp).Left))
            {
                res = new Exp((simplifiedOperator.Left as Exp).Left, new Sub((simplifiedOperator.Left as Exp).Right, (simplifiedOperator.Right as Exp).Right));
            }
            else if (simplifiedOperator.Left is Variable && simplifiedOperator.Right is Variable && (simplifiedOperator.Left as Variable).identifier == (simplifiedOperator.Right as Variable).identifier)
            {
                res = NotNumberOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
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
            return new Div(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Mul(other, Right);
        }
    }

    public class Exp : Operator, IInvertable
    {
        public Exp() : base("^", 40) { }
        public Exp(Expression left, Expression right) : base(left, right, "^", 40) { }

        public override Expression Evaluate()
        {
            return Left ^ Right;
        }

        public override Expression Expand()
        {
            Expression res = null;

            if (Left is Operator && (Left as Operator).priority < priority)
            {
                if (Left is Add)
                {
                    res = new Add(new Add(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right)), new Mul(new Integer(2), new Mul((Left as Operator).Left, (Left as Operator).Right)));
                } 
                else if (Left is Sub)
                {
                    res = new Sub(new Add(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right)), new Mul(new Integer(2), new Mul((Left as Operator).Left, (Left as Operator).Right)));
                }
                else if (Left is Mul)
                {
                    res = new Mul(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right)); 
                }
                else if (Left is Div)
                {
                    res = new Div(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right)); 
                }
            }
            else
            {
                res = new Exp(Left.Expand(), Right.Expand());
            }

            return res;
        }

        public override Expression Simplify()
        {
            Expression evaluatedLeft, evaluatedRight, res = null;
            Operator simplifiedOperator = new Exp(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));

            if (simplifiedOperator.Left is Number && simplifiedOperator.Left.CompareTo(Constant.One))
            {
                return new Integer(1);
            }
            else if (simplifiedOperator.Right is Number && simplifiedOperator.Left.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }
            else if (simplifiedOperator.Left is Variable && simplifiedOperator.Right is Number)
            {
                res = NotNumberOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Number);
            }
            else
            {
                res = simplifiedOperator;
            }

            if (res is Exp && !((evaluatedLeft = (res as Exp).Left.Evaluate()) is Error))
            {
                (res as Exp).Left = evaluatedLeft;
            }

            if (res is Exp && !((evaluatedRight = (res as Exp).Right.Evaluate()) is Error))
            {
                (res as Exp).Right = evaluatedRight;
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
                    if (Left.CompareTo((other as Exp).Left) && Right.CompareTo((other as Exp).Right)) 
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
            return new Exp(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            throw new NotImplementedException();
        }
    }
}