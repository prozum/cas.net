using System;
using System.Collections.Generic;

namespace Ast
{
    // A BinaryOperator which sides can be swapped without effecting the result.
    public interface ISwappable
    {
        Expression Left { get; set; }
        Expression Right { get; set; }

        BinaryOperator Swap();
        BinaryOperator Transform();
        string ToStringParent();
    }


    // A Operator which evaluates two expressions.
    public abstract class BinaryOperator : Expression
    {
        public abstract string Identifier { get; }
        public abstract int Priority { get; }

        public override Scope CurScope
        {
            get { return base.CurScope; }
            set
            {
                base.CurScope = value;
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
            var thisReduced = Reduce();
            var otherReduced = other.Reduce();

            //When this changes type after reduction.
            if (!(thisReduced is BinaryOperator))
            {
                return thisReduced.CompareTo(otherReduced);
            }
            else if (thisReduced is BinaryOperator && otherReduced is BinaryOperator)
            {
                if (thisReduced is ISwappable)
                {
                    return CompareSwappables(thisReduced as ISwappable, otherReduced as BinaryOperator);
                }
                else
                {
                    return CompareSides(thisReduced as BinaryOperator, otherReduced as BinaryOperator);
                }
            }
            else
            {
                var thisEval = thisReduced.Evaluate();
                var otherEval = otherReduced.Evaluate();

                return thisEval.CompareTo(otherEval);
            }
        }

        private bool CompareSwappables(ISwappable exp1, BinaryOperator exp2)
        {
            if ((exp1 as BinaryOperator).Left is ISwappable || (exp1 as BinaryOperator).Right is ISwappable)
            {
                return SwappableCompareAlgorithem(exp1, exp2);
            }
            else if (CompareSides(exp1 as BinaryOperator, exp2) || CompareSides(exp1.Swap().Reduce() as BinaryOperator, exp2))
            {
                return true;
            }

            return false;
        }

        private bool SwappableCompareAlgorithem(ISwappable exp1, BinaryOperator exp2)
        {
            BinaryOperator modified = (exp1 as BinaryOperator).Clone() as BinaryOperator;

            if (modified.ToStringParent() == exp2.ToStringParent())
            {
                return true;
            }

            //Transforms modified until modified == exp2 or modified == exp1
            //When modified == exp2 then exp1s result is the same as exp2, aka they are the same
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
            Expression res = ReduceHelper(Left.Reduce(), Right.Reduce());

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

        #region ModWith
        public override Expression ModWith(Integer other)
        {
            return Evaluate() % other;
        }

        public override Expression ModWith(Rational other)
        {
            return Evaluate() % other;
        }

        public override Expression ModWith(Irrational other)
        {
            return Evaluate() % other;
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