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

        public override void SetFunctionCall(UserDefinedFunction functionCall)
        {
            Left.SetFunctionCall(functionCall);
            Right.SetFunctionCall(functionCall);
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
            return new Equal(Left.Expand(), Right.Expand());
        }

        public override Expression Simplify()
        {
            return new Equal(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));
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

        public override Expression Expand()
        {
            return new Assign(Left.Expand(), Right.Expand());
        }

        public override Expression Simplify()
        {
            return new Assign(Evaluator.SimplifyExp(Left), Evaluator.SimplifyExp(Right));
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
            return new Add(Left.Expand(), Right.Expand());
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
            else if ((simplifiedOperator.Left is Variable && simplifiedOperator.Right is Variable) && CompareVariables(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable))
            {
                res = VariableOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
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
                    return new Add(Evaluator.SimplifyExp(new Mul(new Integer(2), other)), Right);
                }
                else if (Right.CompareTo(other))
                {
                    return new Add(Left, Evaluator.SimplifyExp(new Mul(new Integer(2), other)));
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
            return new Sub(Left.Expand(), Right.Expand());
        }

        public override Expression Simplify()
        {
            Right = Evaluator.SimplifyExp(new Mul(new Integer(-1), Right));
            return new Add(Left, Right).Simplify();
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
            if (Left is Operator && (Left as Operator).priority < priority)
            {
                if (Left is Add)
                {
                    return new Add(new Mul((Left as Operator).Left, Right), new Mul((Left as Operator).Right, Right));
                } 
                else if (Left is Sub)
                {
                    return new Sub(new Mul((Left as Operator).Left, Right), new Mul((Left as Operator).Right, Right));
                }
                else
                {
                    return new Mul(Left.Expand(), Right.Expand());
                }
            } 
            else if (Right is Operator && (Right as Operator).priority < priority)
            {
                if (Right is Add)
                {
                    return new Add(new Mul((Right as Operator).Left, Left), new Mul((Right as Operator).Right, Left));
                }
                else if (Right is Sub)
                {
                    return new Sub(new Mul((Right as Operator).Left, Left), new Mul((Right as Operator).Right, Left));
                }
                else
                {
                    return new Mul(Left.Expand(), Right.Expand());
                }
            }
            else
            {
                return new Mul(Left.Expand(), Right.Expand());
            }
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
                if (CompareVariables(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable))
                {
                    res = SameVariableOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
                }
                else if (!(simplifiedOperator.Left as Variable).exponent.CompareTo(Constant.One) && (simplifiedOperator.Left as Variable).exponent.CompareTo((simplifiedOperator.Right as Variable).exponent))
                {
                    res = DifferentVariableOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
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
                    return new Mul(Evaluator.SimplifyExp(new Exp(other, new Integer(2))), Right);
                }
                else if (Right.CompareTo(other))
                {
                    return new Mul(Left, Evaluator.SimplifyExp(new Mul(other, new Integer(2))));
                }
                else
                {
                    return new Mul(this, other);
                }
            }
        }

        private Expression SImplifyMultiMul(Number other)
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

        private Expression SImplifyMultiMul(Variable other)
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
            if (Left is Operator && (Left as Operator).priority < priority)
            {
                if (Left is Add)
                {
                    return new Add(new Div((Left as Operator).Left, Right), new Div((Left as Operator).Right, Right));
                }
                else if (Left is Sub)
                {
                    return new Sub(new Div((Left as Operator).Left, Right), new Div((Left as Operator).Right, Right));
                }
                else
                {
                    return new Div(Left.Expand(), Right.Expand());
                }
            }
            else if (Right is Operator && (Right as Operator).priority < priority)
            {
                if (Right is Add)
                {
                    return new Add(new Div((Right as Operator).Left, Left), new Div((Right as Operator).Right, Left));
                }
                else if (Right is Sub)
                {
                    return new Sub(new Div((Right as Operator).Left, Left), new Div((Right as Operator).Right, Left));
                }
                else
                {
                    return new Div(Left.Expand(), Right.Expand());
                }
            }
            else
            {
                return new Div(Left.Expand(), Right.Expand());
            }
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
            else if (simplifiedOperator.Left is Variable && simplifiedOperator.Right is Variable && CompareVariables(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable))
            {
                res = VariableOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Variable);
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
            if (Left is Operator && (Left as Operator).priority < priority)
            {
                if (Left is Add)
                {
                    return new Add(new Add(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right)), new Mul(new Integer(2), new Mul((Left as Operator).Left, (Left as Operator).Right)));
                } 
                else if (Left is Sub)
                {
                    return new Sub(new Add(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right)), new Mul(new Integer(2), new Mul((Left as Operator).Left, (Left as Operator).Right)));
                }
                else if (Left is Mul)
                {
                    return new Mul(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right)); 
                }
                else if (Left is Div)
                {
                    return new Div(new Exp((Left as Operator).Left, Right), new Exp((Left as Operator).Right, Right));
                }
                else
                {
                    return new Exp(Left.Expand(), Right.Expand());
                }
            }
            else
            {
                return new Exp(Left.Expand(), Right.Expand());
            }
        }

        public override Expression Simplify()
        {
            Expression evaluatedRes, res = null;
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
                res = VariableOperation(simplifiedOperator.Left as Variable, simplifiedOperator.Right as Number);
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

        private Variable VariableOperation(Variable left, Number right)
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