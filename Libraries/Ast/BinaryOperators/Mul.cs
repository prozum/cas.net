using System;

namespace Ast
{
    public class Mul : BinaryOperator, ISwappable, IInvertable
    {
        public override string Identifier { get { return "*"; } }
        public override int Priority { get{ return 40; } }

        public Mul() { }
        public Mul(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
        {
            return Left * Right;
        }

        //Multiplies one side into the other, it is Add or Sub. (x+y)*z -> xz + yz
        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is Add)
            {
                return new Add(new Mul((left as BinaryOperator).Left, right).Reduce(), new Mul((left as BinaryOperator).Right, right).Reduce());
            }
            else if (left is Sub)
            {
                return new Sub(new Mul((left as BinaryOperator).Left, right).Reduce(), new Mul((left as BinaryOperator).Right, right).Reduce());
            }
            else if (right is Add)
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

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            if (left is Real)
            {
                //Left is zero, return 0. 0*x -> 0
                if (left.CompareTo(Constant.Zero))
                {
                    return new Integer(0);
                }
                //Left is one, return right. 1*x -> x
                else if (left.CompareTo(Constant.One))
                {
                    return right;
                }
                //Right is Variable. Multiplies left with right's prefix. 4*3x -> (4*3)x
                else if (right is Variable)
                {
                    var res = right.Clone();
                    (res as Variable).Prefix = ((res as Variable).Prefix * left) as Real;
                    return res;
                }
            }
            else if (right is Real)
            {
                //Right is zero, return 0. x*0 -> 0
                if (right.CompareTo(Constant.Zero))
                {
                    return new Integer(0);
                }
                //Right is one, return right. x*1 -> x
                else if (right.CompareTo(Constant.One))
                {
                    return left;
                }
                //Left is Variable. Multiplies right with left's prefix. 3x*4 -> (4*3)x
                else if (left is Variable)
                {
                    var res = left.Clone();
                    (res as Variable).Prefix = ((res as Variable).Prefix * right) as Real;
                    return res;
                }
            }
            //Both real, calculate. 5*5 -> 25.
            else if (right is Real)
            {
                return left * right;
            }
            //When left is Mul, go into that Mul, and check if right can be reduced with left's sides.
            else if (left is Mul)
            {
                return (left as Mul).ReduceMultiMul(right);
            }
            //When right is Mul, go into that Mul, and check if left can be reduced with right's sides.
            else if (right is Mul)
            {
                return (right as Mul).ReduceMultiMul(left);
            }
            //When left is Div, change tree. (y/z)*x -> (y*x)/z
            else if (left is Div)
            {
                return new Div(new Mul((left as Div).Left, right), (left as Div).Right);
            }
            //When right is Div, change tree. x*(y/z) -> (x*y)/z
            else if (right is Div)
            {
                return new Div(new Mul((right as Div).Left, left), (right as Div).Right);
            }
            //When left and right is Exp, and their right side is the same: x^z * y^z -> (x*y)^z 
            else if ((left is Exp && right is Exp) && (left as Exp).Right.CompareTo((right as Exp).Right))
            {
                return new Exp(new Mul((left as Exp).Left, (right as Exp).Left), (left as Exp).Right);
            }
            else if (left is Variable && right is Variable)
            {
                //If Variables are the same, Mul their prefixs, and Add their exponents.
                if (CompareVariables(left as Variable, right as Variable))
                {
                    return SameVariableOperation(left as Variable, right as Variable);
                }
                //If Variables are not the same, and their exponents are not one: x^z * y^z -> (x*y)^z 
                else if (!(left as Variable).Exponent.CompareTo(Constant.One) && (left as Variable).Exponent.CompareTo((right as Variable).Exponent))
                {
                    return DifferentVariableOperation(left as Variable, right as Variable);
                }
            }
            //If left and right are the same, return the squared of one of them. (x+y)*(y+x) -> (x+y)^2
            else if (left.CompareTo(right))
            {
                return new Exp(left, new Integer(2));
            }

            //Couldn't reduce.
            return new Mul(left, right);
        }

        private bool CompareVariables(Variable left, Variable right)
        {
            if (left.Identifier == right.Identifier && left.GetType() == right.GetType())
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

        private Expression ReduceMultiMul(dynamic other)
        {
            if (other is Variable || other is Real)
            {
                return ReduceMultiMul(other);
            }
            else
            {
                //If left and other are the same, return the squared of one of them times the right. ((x+y)*z)*(y+x) -> (x+y)^2 * z
                if (Left.CompareTo(other))
                {
                    return new Mul(new Exp(other, new Integer(2)).Reduce(), Right);
                }
                //If right and other are the same, return the squared of one of them times the left. (z*(x+y))*(y+x) -> z * (x+y)^2
                else if (Right.CompareTo(other))
                {
                    return new Mul(Left, new Exp(other, new Integer(2)).Reduce());
                }
                //Couldn't reduce.
                else
                {
                    return new Mul(this, other);
                }
            }
        }

        private Expression ReduceMultiMul(Real other)
        {
            //When other is zero, return 0. (x*z)*0 -> 0
            if (other.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            //When other is zero, return 0. (x*z)*1 -> x*y
            else if (other.CompareTo(Constant.One))
            {
                return this;
            }
            //When left is real, calculate left with other. (5*x)*5 -> 25*x
            else if (Left is Real)
            {
                return new Mul(Left * other, Right);
            }
            //When right is real, calculate right with other. (x*5)*5 -> x*25
            else if (Right is Real)
            {
                return new Mul(Left, Right * other);
            }
            //When left is Variable, multiply left's prefix with other. (2x*?)*5 -> 10x*?
            else if (Left is Variable)
            {
                    var res = Left.Clone();
                    (res as Variable).Prefix = ((res as Variable).Prefix * other) as Real;
                    return new Mul(res, Right);
            }
            //When right is Variable, multiply right's prefix with other. (?*2x)*5 -> ?*10x
            else if (Right is Variable)
            {
                var res = Right.Clone();
                    (res as Variable).Prefix = ((res as Variable).Prefix * other) as Real;
                    return new Mul(Left, res);
            }
            //Couldn't reduce.
            else
            {
                return new Mul(this, other);
            }
        }

        private Expression ReduceMultiMul(Variable other)
        {
            //If left and other are the same Variable, Mul their prefixs, and Add their exponents. (2x^3 * y)*3x^2 -> 6x^5 * y
            if (Left is Variable && CompareVariables(Left as Variable, other))
            {
                return new Mul(SameVariableOperation(Left as Variable, other), Right);
            }
            //If right and other are the same Variable, Mul their prefixs, and Add their exponents. (y * 2x^3)*3x^2 -> y * 6x^5
            else if (Right is Variable && CompareVariables(Right as Variable, other))
            {
                return new Mul(Left, SameVariableOperation(Right as Variable, other));
            }
            //If left and other are not the same Variable, and their exponents are not one: (x^z * q) * y^z -> (x*y)^z * q
            if (Left is Variable && (!(Left as Variable).Exponent.CompareTo(Constant.One) && (Left as Variable).Exponent.CompareTo((other as Variable).Exponent)))
            {
                return new Mul(DifferentVariableOperation(Left as Variable, other as Variable), Right);
            }
            //If right and other are not the same Variable, and their exponents are not one: (q * x^z) * y^z -> q * (x*y)^z
            else if (Right is Variable && (!(Right as Variable).Exponent.CompareTo(Constant.One) && (Right as Variable).Exponent.CompareTo((other as Variable).Exponent)))
            {
                return new Mul(Left, DifferentVariableOperation(Right as Variable, other as Variable));
            }
            //When left is Mul, go into that Mul, and check if other can be reduced with left's sides.
            else if (Left is Mul)
            {
                var res = new Mul((Left as Mul).ReduceMultiMul(other), Right);

                //If Couldn't reduce
                if (res.ToString() == new Mul(new Mul(Left, other), Right).ToString())
                {
                    res = new Mul(this, other);
                }

                return res;
            }
            //When right is Mul, go into that Mul, and check if other can be reduced with right's sides.
            else if (Right is Mul)
            {
                var res = new Mul(Left, (Right as Mul).ReduceMultiMul(other));

                //If Couldn't reduce
                if (res.ToString() == new Mul(Left, new Mul(Right, other)).ToString())
                {
                    res = new Mul(this, other);
                }

                return res;
            }
            //Couldn't reduce
            else
            {
                return new Mul(this, other);
            }
        }

        //Mul Variables prefixs, and Add Variables exponents, and return a new Variable.
        private Expression SameVariableOperation(Variable left, Variable right)
        {
            var res = left.Clone() as Variable;

            res.Prefix = (left.Prefix * right.Prefix) as Real;
            res.Exponent = (left.Exponent + right.Exponent) as Real;

            return res;
        }

        //x^z * y^z -> (x*y)^z 
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
            //When left is Mul, make right Mul instead. (x*y)*z -> x*(y*z)
            if (Left is Mul)
            {
                return new Mul((Left as Mul).Left, new Mul((Left as Mul).Right, Right));
            }
            //When right is Mul, make left Mul instead. x*(y*z) -> (x*y)*z
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

