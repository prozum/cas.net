﻿using System;

namespace Ast
{
    public class Exp : Operator, IInvertable
    {
        public Exp() : base("^", 50) { }
        public Exp(Expression left, Expression right) : base(left, right, "^", 40) { }

        public override Expression Evaluate()
        {
            return Left ^ Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    return new Add(new Add(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right)), new Mul(new Integer(2), new Mul((left as Operator).Left, (left as Operator).Right)));
                }
                else if (left is Sub)
                {
                    return new Sub(new Add(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right)), new Mul(new Integer(2), new Mul((left as Operator).Left, (left as Operator).Right)));
                }
                else if (left is Mul)
                {
                    return new Mul(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right));
                }
                else if (left is Div)
                {
                    return new Div(new Exp((left as Operator).Left, right), new Exp((left as Operator).Right, right));
                }
                else
                {
                    return new Exp(left.Expand(), right.Expand());
                }
            }
            else
            {
                return new Exp(left.Expand(), right.Expand());
            }
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            if (left is Number && left.CompareTo(Constant.One))
            {
                return new Integer(1);
            }
            else if (right is Number && left.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }
            else if (left is Number && right is Number)
            {
                return left ^ right;
            }
            else if (left is Variable && right is Number)
            {
                return VariableOperation(left as Variable, right as Number).Simplify();
            }

            return new Exp(left, right);
        }

        private Variable VariableOperation(Variable left, Number right)
        {
            Variable res = left.Clone() as Variable;

            res.exponent = (left.exponent * right) as Number;

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

        public override Expression CurrectOperator()
        {
            return new Exp(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}
