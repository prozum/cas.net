using System;

namespace Ast
{
    public class Add : Operator, ISwappable, IInvertable
    {
        public Add() : base("+", 20) { }
        public Add(Expression left, Expression right) : base(left, right, "+", 20) { }

        protected override Expression Evaluate(Expression caller)
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
                    return new Add(new Mul(new Integer(2), other), Right);
                }
                else if (Right.CompareTo(other))
                {
                    return new Add(Left, new Mul(new Integer(2), other));
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
}