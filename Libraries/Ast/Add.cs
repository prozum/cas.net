using System;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class Add : BinaryOperator, ISwappable, IInvertable
    {
        public override string Identifier { get { return "+"; } }
        public override int Priority { get{ return 30; } }

        public Add() { }
        public Add(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return Left + Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Add(left.Expand(), right.Expand());
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            if (left is Real && left.CompareTo(Constant.Zero))
            {
                return right;
            }
            else if (right is Real && right.CompareTo(Constant.Zero))
            {
                return left;
            }
            else if (left is Real && right is Real)
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
            return left.Identifier == right.Identifier && left.Exponent.CompareTo(right.Exponent) && left.GetType() == right.GetType();
        }

        private Expression ReduceMultiAdd(dynamic other)
        {
            if (other is Variable || other is Real)
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

        private Expression ReduceMultiAdd(Real other)
        {
            if (Left is Real)
            {
                return new Add(Left + other, Right);
            }
            else if (Right is Real)
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

            (res as Variable).Prefix = (left.Prefix + right.Prefix) as Real;

            return res;
        }

        internal override Expression CurrectOperator()
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

        /// <summary>
        /// 
        /// </summary>
        public Expression InvertOn(Expression other)
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