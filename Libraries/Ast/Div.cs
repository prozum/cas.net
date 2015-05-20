﻿using System;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class Div : BinaryOperator, IInvertable
    {
        public override string Identifier { get { return "/"; } }
        public override int Priority { get{ return 40; } }

        public Div() { }
        public Div(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return Left / Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is BinaryOperator && (left as BinaryOperator).Priority < Priority)
            {
                if (left is Add)
                {
                    return new Add(new Div((left as BinaryOperator).Left, right).Reduce(), new Div((left as BinaryOperator).Right, right).Reduce());
                }
                else if (left is Sub)
                {
                    return new Sub(new Div((left as BinaryOperator).Left, right).Reduce(), new Div((left as BinaryOperator).Right, right).Reduce());
                }
                else
                {
                    return new Div(left.Expand(), right.Expand());
                }
            }
            else if (right is BinaryOperator && (right as BinaryOperator).Priority < Priority)
            {
                if (right is Add)
                {
                    return new Add(new Div((right as BinaryOperator).Left, left).Reduce(), new Div((right as BinaryOperator).Right, Left).Reduce());
                }
                else if (right is Sub)
                {
                    return new Sub(new Div((right as BinaryOperator).Left, left).Reduce(), new Div((right as BinaryOperator).Right, Left).Reduce());
                }
                else
                {
                    return new Div(left.Expand(), right.Expand());
                }
            }
            else
            {
                return new Div(left.Expand(), right.Expand());
            }
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            if (right is Real && right.CompareTo(Constant.One))
            {
                return left;
            }
            else if (left is Real && right is Real)
            {
                return left / right;
            }
            else if (left is Div)
            {
                return new Div((left as Div).Left, new Mul((left as Div).Right, right));
            }
            else if (right is Div)
            {
                return new Div(new Mul(left, (right as Div).Right), (right as Div).Left);
            }
            else if ((left is Exp && right is Exp) && (left as Exp).Left.CompareTo((right as Exp).Left))
            {
                return new Exp((left as Exp).Left, new Sub((left as Exp).Right, (right as Exp).Right));
            }
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

            if (((left.Exponent < right.Exponent) as Boolean).@bool)
            {
                var symbol = right.Clone();

                (symbol as Variable).Exponent = (right.Exponent - left.Exponent) as Real;
                res = new Div(left.Prefix, symbol);
            }
            else if (((left.Exponent > right.Exponent) as Boolean).@bool)
            {
                var symbol = right.Clone();

                (symbol as Variable).Exponent = (left.Exponent - right.Exponent) as Real;
                res = new Div(symbol, right.Prefix);
            }
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

