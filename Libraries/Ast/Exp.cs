using System;
using System.Collections.Generic;

namespace Ast
{
    public class Exp : BinaryOperator, IInvertable
    {
        public Exp() : base("^", 60) { }
        public Exp(Expression left, Expression right) : base(left, right, "^", 60) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left ^ Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is BinaryOperator && (left as BinaryOperator).priority < priority)
            {
                if (left is Add)
                {
                    return new Add(new Add(new Exp((left as BinaryOperator).Left, right).Reduce(), new Exp((left as BinaryOperator).Right, right).Reduce()), new Mul(new Integer(2), new Mul((left as BinaryOperator).Left, (left as BinaryOperator).Right)).Reduce());
                }
                else if (left is Sub)
                {
                    return new Sub(new Add(new Exp((left as BinaryOperator).Left, right).Reduce(), new Exp((left as BinaryOperator).Right, right).Reduce()), new Mul(new Integer(2), new Mul((left as BinaryOperator).Left, (left as BinaryOperator).Right)).Reduce());
                }
                else if (left is Mul)
                {
                    return new Mul(new Exp((left as BinaryOperator).Left, right).Reduce(), new Exp((left as BinaryOperator).Right, right).Reduce());
                }
                else if (left is Div)
                {
                    return new Div(new Exp((left as BinaryOperator).Left, right).Reduce(), new Exp((left as BinaryOperator).Right, right).Reduce());
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

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            if (left is Real && left.CompareTo(Constant.One))
            {
                return new Integer(1);
            }
            else if (right is Real && left.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }
            else if (left is Real && right is Real)
            {
                return left ^ right;
            }
            else if (left is Variable && right is Real)
            {
                return VariableOperation(left as Variable, right as Real);
            }

            return new Exp(left, right);
        }

        private Variable VariableOperation(Variable left, Real right)
        {
            Variable res = left.Clone() as Variable;

            res.exponent = (left.exponent * right) as Real;

            return res;
        }

        public override Expression Clone()
        {
            return new Exp(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            if (Right.CompareTo(Constant.Two))
            {
                var args = new List<Expression>();
                args.Add(other);

                var answer = new Sqrt(args, other.scope);
                var answers = new Ast.List();

                answers.items.Add(answer);
                answers.items.Add(new Mul(new Integer(-1), answer).Reduce());
                return answers;
            }
            else
            {
                return null;
            }
        }

        public override Expression CurrectOperator()
        {
            return new Exp(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

