using System;
using System.Collections.Generic;

namespace Ast
{
    /// <summary>
    /// A BinaryOperator which sides can be swapped without effecting the result.
    /// </summary>
    public interface ISwappable
    {
        Expression Left { get; set; }
        Expression Right { get; set; }

        BinaryOperator Swap();
        BinaryOperator Transform();
        string ToStringParent();
    }

    /// <summary>
    /// A Operator which evaluates two expressions.
    /// </summary>
    public abstract class BinaryOperator : Operator
    {
        public abstract string Identifier { get; }
        public abstract int Priority { get; }

        private Scope _scope;
        public override Scope CurScope
        {
            get { return _scope; }
            set
            {
                _scope = value;
                if (Left != null && Right != null)
                {
                    Left.CurScope = value;
                    Right.CurScope = value;
                }
            }
        }

        private Expression _left;
        public Expression Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;

                if (_left != null)
                    _left.Parent = this;
            }
        }

        private Expression _right;
        public Expression Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;

                if (_right != null)
                    _right.Parent = this;
            }
        }

        internal BinaryOperator() : this(null, null) { }
        internal BinaryOperator(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            if (Parent is BinaryOperator && Priority < (Parent as BinaryOperator).Priority)
            {
                return '(' + Left.ToString() + Identifier + Right.ToString() + ')';
            } 
                
            return Left.ToString() + Identifier + Right.ToString();
        }

        public string ToStringParent()
        {
            return '(' + Left.ToString() + Identifier + Right.ToString() + ')';
        }

        public override bool CompareTo(Expression other)
        {
            var thisSimplified = Reduce();
            var otherSimplified = other.Reduce();

            //When this changes type after reduction.
            if (!(thisSimplified is BinaryOperator))
            {
                return thisSimplified.CompareTo(otherSimplified);
            }
            else if (thisSimplified is BinaryOperator && otherSimplified is BinaryOperator)
            {
                if (thisSimplified is ISwappable)
                {
                    return CompareSwappables(thisSimplified as ISwappable, otherSimplified as BinaryOperator);
                }
                else
                {
                    return CompareSides(thisSimplified as BinaryOperator, otherSimplified as BinaryOperator);
                }
            }
            else
            {
                var thisEval = thisSimplified.Evaluate();
                var otherEval = otherSimplified.Evaluate();

                return thisEval.CompareTo(otherEval);
            }
        }

        private bool CompareSwappables(ISwappable exp1, BinaryOperator exp2)
        {
            if ((exp1 as BinaryOperator).Left is ISwappable || (exp1 as BinaryOperator).Right is ISwappable)
            {
                return SwappableCompareAlorithem(exp1, exp2);
            }
            else if (CompareSides(exp1 as BinaryOperator, exp2) || CompareSides(exp1.Swap().Reduce() as BinaryOperator, exp2))
            {
                return true;
            }

            return false;
        }

        private bool SwappableCompareAlorithem(ISwappable exp1, BinaryOperator exp2)
        {
            BinaryOperator modified = (exp1 as BinaryOperator).Clone() as BinaryOperator;

            if (modified.ToStringParent() == exp2.ToStringParent())
            {
                return true;
            }

            //Transforms modified until modified == exp2 or modified == exp1
            //When modified == exp2 then exp1's result is the same as exp2, aka they are the same
            //When modified == exp1 then the loop is done, and it found no matching trees.
            do
            {
                //First, transform modified.
                modified = (modified as ISwappable).Transform();

                if (modified.ToStringParent() == exp2.ToStringParent())
                {
                    return true;
                }

                //Second, swap one of the swappable sides.
                if (modified.Left is ISwappable)
                {
                    modified.Left = (modified.Left as ISwappable).Swap();

                    if (modified.ToStringParent() == exp2.ToStringParent())
                    {
                        return true;
                    }
                }
                else if (modified.Right is ISwappable)
                {
                    modified.Right = (modified.Right as ISwappable).Swap();

                    if (modified.ToStringParent() == exp2.ToStringParent())
                    {
                        return true;
                    }
                }
            } while (!(modified.ToStringParent() == exp1.ToStringParent()));

            return false;
        }

        private bool CompareSides(BinaryOperator exp1, BinaryOperator exp2)
        {
            return exp1.Left.CompareTo(exp2.Left) && exp1.Right.CompareTo(exp2.Right);
        }

        public override bool ContainsVariable(Variable other)
        {
            return Left.ContainsVariable(other) || Right.ContainsVariable(other);
        }

        public override Expression Reduce()
        {
            var prev = ToString();
            var prevType = GetType();
            //Reduces the whole expression.
            var res = ReduceHelper(Left.Reduce(), Right.Reduce());

            //If the reduction did something, aka res is different from this, then reduce again.
            if (prevType != res.GetType() || prev != res.ToString())
            {
                res = res.Reduce();
            }

            return res;
        }

        protected virtual Expression ReduceHelper(Expression left, Expression right)
        {
            return this;
        }

        public override Expression Expand()
        {
            var prev = ToString();
            //Expands the whole expression.
            var res = ExpandHelper(Left.Expand(), Right.Expand());

            //If the expandtion did something, aka res is different from this, then reduce again.
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