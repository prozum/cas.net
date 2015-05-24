using System;

namespace Ast
{
    public class Div : BinaryOperator, IInvertable
    {
        public override string Identifier { get { return "/"; } }
        public override int Priority { get{ return 40; } }

        public Div() { }
        public Div(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
        {
            return Left / Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            //When left is add or sub then expand. (x+y)/z -> x/z + y/z
            if (left is Add)
            {
                return new Add(new Div((left as BinaryOperator).Left, right).Reduce(), new Div((left as BinaryOperator).Right, right).Reduce());
            }
            else if (left is Sub)
            {
                return new Sub(new Div((left as BinaryOperator).Left, right).Reduce(), new Div((left as BinaryOperator).Right, right).Reduce());
            }
            //Couldn't expand
            else
            {
                return new Div(left.Expand(), right.Expand());
            }
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            //When right is 1, return left. x/1 -> x
            if (right.CompareTo(Constant.One))
            {
                return left;
            }
            //When both are reals, calculate. 6/3 -> 2
            else if (left is Real && right is Real)
            {
                return left / right;
            }
            //When left is div, convert two divs to one. (x/y)/z -> x/(y*z)
            else if (left is Div)
            {
                return new Div((left as Div).Left, new Mul((left as Div).Right, right));
            }
            //When rihgt is div, convert two divs to one. x/(y/z) -> (x*z)/y
            else if (right is Div)
            {
                return new Div(new Mul(left, (right as Div).Right), (right as Div).Left);
            }
            //When both sides are exp, and their left sides are the same. x^a/x^b -> x^(a-b)
            else if ((left is Exp && right is Exp) && (left as Exp).Left.CompareTo((right as Exp).Left))
            {
                return new Exp((left as Exp).Left, new Sub((left as Exp).Right, (right as Exp).Right));
            }
            //When both sides are Variable, and their exponents are the same. x^a/x^b -> x^(a-b)
            else if (left is Variable && right is Variable && CompareVariables(left as Variable, right as Variable))
            {
                return VariableOperation(left as Variable, right as Variable);
            }

            return new Div(left, right);
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

        private Expression VariableOperation(Variable left, Variable right)
        {
            Expression res;

            //When left exponent is lesser than right exponent. 2x^2/3x^4 -> 2/3x^2
            if (((left.Exponent < right.Exponent) as Boolean).@bool)
            {
                var symbol = right.Clone();

                (symbol as Variable).Exponent = (right.Exponent - left.Exponent) as Real;
                res = new Div(left.Prefix, symbol);
            }
            //When left exponent is greater than right exponent. 2x^4/3x^2 -> 2x^2/3
            else if (((left.Exponent > right.Exponent) as Boolean).@bool)
            {
                var symbol = right.Clone();

                (symbol as Variable).Exponent = (left.Exponent - right.Exponent) as Real;
                res = new Div(symbol, right.Prefix);
            }
            //When left exponent equals right exponent. 6x^3/3x^3 -> 6/3 -> 2
            else
            {
                res = left.Prefix / right.Prefix;
            }

            return res;
        }

        public override Expression Clone()
        {
            return new Div(Left.Clone(), Right.Clone());
        }

        public Expression InvertOn(Expression other)
        {
            return new Mul(other, Right);
        }

        internal override Expression CurrectOperator()
        {
            return new Div(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

