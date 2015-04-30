using System;

namespace Ast
{
    public class Add : BinaryOperator, ISwappable, IInvertable
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

        protected override Expression ReduceHelper(Expression left, Expression right)
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
                return (left as Add).ReduceMultiAdd(right);
            }
            else if (right is Add)
            {
                return (right as Add).ReduceMultiAdd(left);
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

        private Expression ReduceMultiAdd(dynamic other)
        {
            if (other is Variable || other is Number)
            {
                return ReduceMultiAdd(other);
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

        private Expression ReduceMultiAdd(Number other)
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

        private Expression ReduceMultiAdd(Variable other)
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
                var res = new Add((Left as Add).ReduceMultiAdd(other), Right);

                if (res.ToString() == new Add(new Add(Left, other), Right).ToString())
                {
                    res = new Add(this, other);
                }

                return res;
            }
            else if (Right is Add)
            {
                var res = new Add(Left, (Right as Add).ReduceMultiAdd(other));

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
            if (Right is INegative && (Right as INegative).IsNegative())
            {
                return new Sub(Left.CurrectOperator(), (Right as INegative).ToNegative());
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

        public BinaryOperator Swap()
        {
            return new Add(Right, Left);
        }

        public BinaryOperator Transform()
        {
            if (Left is Add)
            {
                return new Add((Left as Add).Left, new Add((Left as Add).Right, Right));
            }
            else if (Right is Add)
            {
                return new Add(new Add(Left, (Right as Add).Left), (Right as Add).Right);
            }
            else
            {
                return this;
            }
        }
    }
}