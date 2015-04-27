using System;

namespace Ast
{
    public class Mul : Operator, ISwappable, IInvertable
    {
        public Mul() : base("*", 30) { }
        public Mul(Expression left, Expression right) : base(left, right, "*", 30) { }

        protected override Expression Evaluate(Expression caller)
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
                    return new Mul(new Exp(other, new Integer(2)).Simplify(this), Right);
                }
                else if (Right.CompareTo(other))
                {
                    return new Mul(Left, new Exp(other, new Integer(2)).Simplify(this));
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
            var res = left.Clone() as Variable;

            res.prefix = (left.prefix * right.prefix) as Number;
            res.exponent = (left.exponent + right.exponent) as Number;

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
}

