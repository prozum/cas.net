using System;

namespace Ast
{
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
            //When left is 0, return right. "0+x -> x"
            if (left.CompareTo(Constant.Zero))
            {
                return right;
            }
            //When right is 0, return left. "x+0 -> x"
            else if (right.CompareTo(Constant.Zero))
            {
                return left;
            }
            //When both sides are real, add them together. "2+2 -> 4"
            else if (left is Real && right is Real)
            {
                return left + right;
            }
            //When left is add, go into that add and check if right can be reduced with left's sides.
            else if (left is Add)
            {
                return (left as Add).ReduceMultiAdd(right);
            }
            //When right is add, go into that add and check if left can be reduced with right's sides.
            else if (right is Add)
            {
                return (right as Add).ReduceMultiAdd(left);
            }
            //When both are the same variable, and there exponent are the same. "x^2 + x^2 -> 2x^2"
            else if ((left is Variable && right is Variable) && CompareVariables(left as Variable, right as Variable))
            {
                return VariableOperation(left as Variable, right as Variable);
            }
            //When the sides are the same. "(x*y)+(y*x) -> 2xy"
            else if (left.CompareTo(Right))
            {
                return new Mul(new Integer(2), left);
            }
            //Couldn't reduce
            else
            {
                return new Add(left, right);
            }
        }

        private bool CompareVariables(Variable left, Variable right)
        {
            if (left.Identifier == right.Identifier && left.Exponent.CompareTo(right.Exponent) && left.GetType() == right.GetType())
            {
                if (left is Func)
                {
                    if ((left as Func).CompareArgsTo(right as Func))
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private Expression ReduceMultiAdd(dynamic other)
        {
            if (other is Variable || other is Real)
            {
                return ReduceMultiAdd(other);
            }
            else
            {
                //When other is the same as left. ((x*y)+y)+(x*y) -> 2(x*y)+y
                if (Left.CompareTo(other))
                {
                    return new Add(new Mul(new Integer(2), other), Right);
                }
                //When other is the same as right. (y+(x*y))+(x*y) -> y+2(x*y)
                else if (Right.CompareTo(other))
                {
                    return new Add(Left, new Mul(new Integer(2), other));
                }
                //Couldn't reduce
                else
                {
                    return new Add(this, other);
                }
            }
        }

        private Expression ReduceMultiAdd(Real other)
        {
            //When left is real, add other to left. (5+x)+5 -> 10+x
            if (Left is Real)
            {
                return new Add(Left + other, Right);
            }
            //When right is real, add other to right. (x+5)+5 -> x+10
            else if (Right is Real)
            {
                return new Add(Left, Right + other);
            }
            //Couldn't reduce
            else
            {
                return new Add(this, other);
            }
        }

        private Expression ReduceMultiAdd(Variable other)
        {
            //When left and other are the same variable, and there exponent are the same. "(x^2+x)+x^2 -> 2x^2+x"
            if (Left is Variable && CompareVariables(Left as Variable, other))
            {
                return new Add(VariableOperation(Left as Variable, other), Right);
            }
            //When right and other are the same variable, and there exponent are the same. "(x+x^2)+x^2 -> x+2x^2"
            else if (Right is Variable && CompareVariables(Right as Variable, other))
            {
                return new Add(Left, VariableOperation(Right as Variable, other));
            }
            //When left is add, go into that add and check if other can be reduced with left's sides.
            else if (Left is Add)
            {
                var res = new Add((Left as Add).ReduceMultiAdd(other), Right);

                //If true then Couldn't reduce
                if (res.ToString() == new Add(new Add(Left, other), Right).ToString())
                {
                    res = new Add(this, other);
                }

                return res;
            }
            //When right is add, go into that add and check if other can be reduced with right's sides.
            else if (Right is Add)
            {
                var res = new Add(Left, (Right as Add).ReduceMultiAdd(other));

                //If true then Couldn't reduce
                if (res.ToString() == new Add(Left, new Add(Right, other)).ToString())
                {
                    res = new Add(this, other);
                }

                return res;
            }
            //Couldn't reduce
            else
            {
                return new Add(this, other);
            }
        }

        //Returns to Variables added together. 2x+3x -> 5x
        private Expression VariableOperation(Variable left, Variable right)
        {
            var res = left.Clone();

            (res as Variable).Prefix = (left.Prefix + right.Prefix) as Real;

            return res;
        }

        internal override Expression CurrectOperator()
        {
            //When right is negative, return a sub, with right as positive. 2+(-2) -> 2-2.
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
            //When left is add, make right add instead. (x+y)+z -> x+(y+z)
            if (Left is Add)
            {
                return new Add((Left as Add).Left, new Add((Left as Add).Right, Right));
            }
            //When right is add, make right add instead. x+(y+z) -> (x+y)+z
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