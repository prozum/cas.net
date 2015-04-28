using System;
using System.Collections.Generic;

namespace Ast
{
    public interface ISwappable
    {
        Operator Swap();
        Operator Transform();
    }

    public abstract class Operator : Expression
    {
        public string symbol;
        public int priority;

        private Expression left;
        public Expression Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;

                if (left != null)
                    left.parent = this;
            }
        }

        private Expression right;
        public Expression Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;

                if (right != null)
                    right.parent = this;
            }
        }

        public Operator(string symbol, int priority) : this(null, null, symbol, priority) { }
        public Operator(Expression left, Expression right, string symbol, int priority)
        {
            Left = left;
            Right = right;
            this.symbol = symbol;
            this.priority = priority;
        }

        protected override Expression Evaluate(Expression caller)
        {
            return new Error(this, "Cannot evaluate operator expression!");
        }

        public override string ToString()
        {
            if (parent == null || priority >= parent.priority)
            {
                return Left.ToString () + symbol + Right.ToString ();
            } 
            else 
            {
                return '(' + Left.ToString () + symbol + Right.ToString () + ')';
            }
        }

        public override bool CompareTo(Expression other)
        {
            Expression thisSimplified = Simplify();
            Expression otherSimplified = other.Simplify();

            if (!(thisSimplified is Operator))
            {
                return thisSimplified.CompareTo(otherSimplified);
            }
            else if (thisSimplified is Operator && otherSimplified is Operator)
            {
                if (thisSimplified is ISwappable)
                {
                    return CompareSwappables(thisSimplified as ISwappable, otherSimplified as Operator);
                }
                else
                {
                    return CompareSides(thisSimplified as Operator, otherSimplified as Operator);
                }
            }
            else
            {
                return false;
            }
        }

        private bool CompareSwappables(ISwappable exp1, Operator exp2)
        {
            if (CompareSides(exp1 as Operator, exp2) || CompareSides(exp1.Swap().Simplify() as Operator, exp2))
            {
                return true;
            }
            else if ((exp1 as Operator).Left is ISwappable || (exp1 as Operator).Right is ISwappable)
            {
                return SwappableCompareAlorithem(exp1, exp2);
            }

            return false;
        }

        private bool SwappableCompareAlorithem(ISwappable exp1, Operator exp2)
        {
            Operator modified = (exp1 as Operator).Clone() as Operator;

            do
            {
                modified = (modified as ISwappable).Transform();

                if (modified.CompareTo(exp2))
                {
                    return true;
                }

                if (modified.Left is ISwappable)
                {
                    modified.Left = (modified.Left as ISwappable).Swap();

                    if (modified.CompareTo(exp2))
                    {
                        return true;
                    }
                }
                else if (modified.Right is ISwappable)
                {
                    modified.Right = (modified.Right as ISwappable).Swap();

                    if (modified.CompareTo(exp2))
                    {
                        return true;
                    }
                }
            } while (!modified.CompareTo(exp1 as Operator));

            return false;
        }

        private bool CompareSides(Operator exp1, Operator exp2)
        {
            return exp1.Left.CompareTo(exp2.Left) && exp1.Right.CompareTo(exp2.Right);
        }

        public override bool ContainsVariable(Variable other)
        {
            return Left.ContainsVariable(other) || Right.ContainsVariable(other);
        }

        internal override Expression Simplify(Expression caller)
        {
            var prev = ToString();
            var prevType = GetType();
            var res = SimplifyHelper(Left.Simplify(caller), Right.Simplify(caller));

            if (prevType != res.GetType() || prev != res.ToString())
            {
                res = res.Simplify(caller);
            }

            return res;
        }

        protected virtual Expression SimplifyHelper(Expression left, Expression right)
        {
            return this;
        }

        public override Expression Expand()
        {
            var prev = ToString();
            var res = ExpandHelper(Left.Expand(), Right.Expand());

            if (prev != res.ToString())
            {
                res = res.Expand();
            }

            return res;
        }

        protected virtual Expression ExpandHelper(Expression left, Expression right)
        {
            return this;
        }

        #region AddWith
        public override Expression AddWith(Integer other)
        {
            return Evaluate() + other;
        }

        public override Expression AddWith(Rational other)
        {
            return Evaluate() + other;
        }

        public override Expression AddWith(Irrational other)
        {
            return Evaluate() + other;
        }

        public override Expression AddWith(Text other)
        {
            return new Text(Evaluate().ToString() + other);
        }

        #endregion

        #region SubWith
        public override Expression SubWith(Integer other)
        {
            return Evaluate() - other;
        }

        public override Expression SubWith(Rational other)
        {
            return Evaluate() - other;
        }

        public override Expression SubWith(Irrational other)
        {
            return Evaluate() - other;
        }

        #endregion

        #region MulWith
        public override Expression MulWith(Integer other)
        {
            return Evaluate() * other;
        }

        public override Expression MulWith(Rational other)
        {
            return Evaluate() * other;
        }

        public override Expression MulWith(Irrational other)
        {
            return Evaluate() * other;
        }

        #endregion

        #region DivWith
        public override Expression DivWith(Integer other)
        {
            return Evaluate() / other;
        }

        public override Expression DivWith(Rational other)
        {
            return Evaluate() / other;
        }

        public override Expression DivWith(Irrational other)
        {
            return Evaluate() / other;
        }

        #endregion

        #region ExpWith
        public override Expression ExpWith(Integer other)
        {
            return Evaluate() ^ other;
        }

        public override Expression ExpWith(Rational other)
        {
            return Evaluate() ^ other;
        }

        public override Expression ExpWith(Irrational other)
        {
            return Evaluate() ^ other;
        }

        #endregion

        #region GreaterThan
        public override Expression GreaterThan(Integer other)
        {
            return Evaluate() > other;
        }

        public override Expression GreaterThan(Rational other)
        {
            return Evaluate() > other;
        }

        public override Expression GreaterThan(Irrational other)
        {
            return Evaluate() > other;
        }

        #endregion

        #region LesserThan
        public override Expression LesserThan(Integer other)
        {
            return Evaluate() < other;
        }

        public override Expression LesserThan(Rational other)
        {
            return Evaluate() < other;
        }

        public override Expression LesserThan(Irrational other)
        {
            return Evaluate() < other;
        }

        #endregion

        #region GreaterThanOrEqualTo
        public override Expression GreaterThanOrEqualTo(Integer other)
        {
            return Evaluate() >= other;
        }

        public override Expression GreaterThanOrEqualTo(Rational other)
        {
            return Evaluate() >= other;
        }

        public override Expression GreaterThanOrEqualTo(Irrational other)
        {
            return Evaluate() >= other;
        }

        #endregion

        #region LesserThanOrEqualTo
        public override Expression LesserThanOrEqualTo(Integer other)
        {
            return Evaluate() <= other;
        }

        public override Expression LesserThanOrEqualTo(Rational other)
        {
            return Evaluate() <= other;
        }

        public override Expression LesserThanOrEqualTo(Irrational other)
        {
            return Evaluate() <= other;
        }

        #endregion
    }
}