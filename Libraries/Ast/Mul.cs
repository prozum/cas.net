using System;

namespace Ast
{
    public class Mul : BinaryOperator, ISwappable, IInvertable
    {
        public override string Identifier { get { return "*"; } }
        public override int Priority { get{ return 40; } }

        public Mul() { }
        public Mul(Expression left, Expression right) : base(left, right) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left * Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is BinaryOperator && (left as BinaryOperator).Priority < Priority)
            {
                if (left is Add)
                {
                    return new Add(new Mul((left as BinaryOperator).Left, right).Reduce(), new Mul((left as BinaryOperator).Right, right).Reduce());
                }
                else if (left is Sub)
                {
                    return new Sub(new Mul((left as BinaryOperator).Left, right).Reduce(), new Mul((left as BinaryOperator).Right, right).Reduce());
                }
                else
                {
                    return new Mul(left.Expand(), right.Expand());
                }
            }
            else if (right is BinaryOperator && (right as BinaryOperator).Priority < Priority)
            {
                if (right is Add)
                {
                    return new Add(new Mul((right as BinaryOperator).Left, left).Reduce(), new Mul((right as BinaryOperator).Right, left).Reduce());
                }
                else if (right is Sub)
                {
                    return new Sub(new Mul((right as BinaryOperator).Left, left).Reduce(), new Mul((right as BinaryOperator).Right, left).Reduce());
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

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            if (left is Real)
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
                    (res as Variable).Prefix = ((res as Variable).Prefix * left) as Real;
                    return res;
                }
                else if (right is Real)
                {
                    return left * right;
                }
            }
            else if (right is Real)
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
                    (res as Variable).Prefix = ((res as Variable).Prefix * right) as Real;
                    return res;
                }
                else if (left is Real)
                {
                    return left * right;
                }
            }
            else if (left is Mul)
            {
                return (left as Mul).ReduceMultiMul(right);
            }
            else if (right is Mul)
            {
                return (right as Mul).ReduceMultiMul(left);
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
                else if (!(left as Variable).Exponent.CompareTo(Constant.One) && (left as Variable).Exponent.CompareTo((right as Variable).Exponent))
                {
                    return DifferentVariableOperation(left as Variable, right as Variable);
                }
            }

            return new Mul(left, right);
        }

        private bool CompareVariables(Variable left, Variable right)
        {
            return left.Identifier == right.Identifier && left.GetType() == right.GetType();
        }

        private Expression ReduceMultiMul(dynamic other)
        {
            if (other is Variable || other is Real)
            {
                return ReduceMultiMul(other);
            }
            else
            {
                if (Left.CompareTo(other))
                {
                    return new Mul(new Exp(other, new Integer(2)).Reduce(this), Right);
                }
                else if (Right.CompareTo(other))
                {
                    return new Mul(Left, new Exp(other, new Integer(2)).Reduce(this));
                }
                else
                {
                    return new Mul(this, other);
                }
            }
        }

        private Expression ReduceMultiMul(Real other)
        {
            if (other.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            else if (other.CompareTo(Constant.One))
            {
                return this;
            }
            else if (Left is Real)
            {
                return new Mul(Left * other, Right);
            }
            else if (Right is Real)
            {
                return new Mul(Left, Right * other);
            }
            else
            {
                return new Mul(this, other);
            }
        }

        private Expression ReduceMultiMul(Variable other)
        {
            if (Left is Variable && CompareVariables(Left as Variable, other))
            {
                return new Mul(SameVariableOperation(Left as Variable, other), Right);
            }
            else if (Right is Variable && CompareVariables(Right as Variable, other))
            {
                return new Mul(Left, SameVariableOperation(Right as Variable, other));
            }
            if (Left is Variable && (!(Left as Variable).Exponent.CompareTo(Constant.One) && (Left as Variable).Exponent.CompareTo((other as Variable).Exponent)))
            {
                return new Mul(DifferentVariableOperation(Left as Variable, other as Variable), Right);
            }
            else if (Right is Variable && (!(Right as Variable).Exponent.CompareTo(Constant.One) && (Right as Variable).Exponent.CompareTo((other as Variable).Exponent)))
            {
                return new Mul(Left, DifferentVariableOperation(Right as Variable, other as Variable));
            }
            else if (Left is Mul)
            {
                var res = new Mul((Left as Mul).ReduceMultiMul(other), Right);

                if (res.ToString() == new Mul(new Mul(Left, other), Right).ToString())
                {
                    res = new Mul(this, other);
                }

                return res;
            }
            else if (Right is Mul)
            {
                var res = new Mul(Left, (Right as Mul).ReduceMultiMul(other));

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

            res.Prefix = (left.Prefix * right.Prefix) as Real;
            res.Exponent = (left.Exponent + right.Exponent) as Real;

            return res;
        }

        private Expression DifferentVariableOperation(Variable left, Variable right)
        {
            var newLeft = left.Clone();
            var newRight = right.Clone();
            (newLeft as Variable).Exponent = new Integer(1);
            (newRight as Variable).Exponent = new Integer(1);

            return new Mul(left.Prefix * right.Prefix, new Exp(new Mul(newLeft, newRight), left.Exponent));
        }

        public override Expression Clone()
        {
            return new Mul(Left.Clone(), Right.Clone());
        }

        public Expression InvertOn(Expression other)
        {
            return new Div(other, Right);
        }

        internal override Expression CurrectOperator()
        {
            return new Mul(Left.CurrectOperator(), Right.CurrectOperator());
        }

        public BinaryOperator Swap()
        {
            return new Mul(Right, Left);
        }

        public BinaryOperator Transform()
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

