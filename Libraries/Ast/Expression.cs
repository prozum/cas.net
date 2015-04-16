using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
    public abstract class Expression
    {
        public Evaluator evaluator;
        public Operator parent;
        public UserDefinedFunction functionCall;
        public abstract Expression Evaluate();

        public virtual void SetFunctionCall(UserDefinedFunction functionCall)
        {
            this.functionCall = functionCall;
        }

        public virtual Expression Expand()
        {
            return this;
        }

        public virtual Expression Simplify()
        {
            return this;
        }

        public virtual bool CompareTo(Expression other)
        {
            if (this.GetType() == other.GetType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract bool ContainsNotNumber(NotNumber other);

        //public abstract string ToString ();

        #region AddWith
        public virtual Expression AddWith(Integer other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(Rational other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(Irrational other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(Boolean other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(Complex other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(NotNumber other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression AddWith(Operator other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression AddWith(Message other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(List other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        #endregion

        #region SubWith
        public virtual Expression SubWith(Integer other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(Rational other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(Irrational other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(Boolean other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(Complex other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(NotNumber other)
        {
            return this - other.Evaluate();
        }

        public virtual Expression SubWith(Operator other)
        {
            return this - other.Evaluate();
        }

        public virtual Expression SubWith(Message other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(List other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        #endregion

        #region MulWith
        public virtual Expression MulWith(Integer other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(Rational other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(Irrational other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(Boolean other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(Complex other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(NotNumber other)
        {
            return this * other.Evaluate();
        }

        public virtual Expression MulWith(Operator other)
        {
            return this * other.Evaluate();
        }

        public virtual Expression MulWith(Message other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(List other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        #endregion

        #region DivWith
        public virtual Expression DivWith(Integer other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(Rational other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(Irrational other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(Boolean other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(Complex other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(NotNumber other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression DivWith(Operator other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression DivWith(Message other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(List other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        #endregion

        #region ExpWith
        public virtual Expression ExpWith(Integer other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(Rational other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(Irrational other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(Boolean other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(Complex other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(NotNumber other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression ExpWith(Operator other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression ExpWith(Message other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(List other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        #endregion

        #region GreaterThan
        public virtual Expression GreaterThan(Integer other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(Rational other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(Irrational other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(Boolean other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(Complex other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(NotNumber other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression GreaterThan(Operator other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression GreaterThan(Message other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(List other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        #endregion

        #region LesserThan
        public virtual Expression LesserThan(Integer other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(Rational other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(Irrational other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(Boolean other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(Complex other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(NotNumber other)
        {
            return this < other.Evaluate();
        }

        public virtual Expression LesserThan(Operator other)
        {
            return this < other.Evaluate();
        }

        public virtual Expression LesserThan(Message other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(List other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        #endregion

        #region GreaterThanOrEqualTo
        public virtual Expression GreaterThanOrEqualTo(Integer other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(Rational other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(Irrational other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(Boolean other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(Complex other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(NotNumber other)
        {
            return this >= other.Evaluate();
        }

        public virtual Expression GreaterThanOrEqualTo(Operator other)
        {
            return this >= other.Evaluate();
        }

        public virtual Expression GreaterThanOrEqualTo(Message other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(List other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        #endregion

        #region LesserThanOrEqualTo
        public virtual Expression LesserThanOrEqualTo(Integer other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(Rational other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(Irrational other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(Boolean other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(Complex other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(NotNumber other)
        {
            return this <= other.Evaluate();
        }

        public virtual Expression LesserThanOrEqualTo(Operator other)
        {
            return this <= other.Evaluate();
        }

        public virtual Expression LesserThanOrEqualTo(Message other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(List other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        #endregion

        public static Expression operator +(Expression left, dynamic right)
        {
            return left.AddWith(right);
        }

        public static Expression operator -(Expression left, dynamic right)
        {
            return left.SubWith(right);
        }

        public static Expression operator *(Expression left, dynamic right)
        {
            return left.MulWith(right);
        }

        public static Expression operator /(Expression left, dynamic right)
        {
            if (right.CompareTo(new Integer(0)))
            {
                return new Error(left, "Cannot be divided by 0");
            }

            return left.DivWith(right);
        }

        public static Expression operator ^(Expression left, dynamic right)
        {
            return left.ExpWith(right);
        }

        public static Expression operator >(Expression left, dynamic right)
        {
            return left.GreaterThan(right);
        }

        public static Expression operator <(Expression left, dynamic right)
        {
            return left.LesserThan(right);
        }

        public static Expression operator >=(Expression left, dynamic right)
        {
            return left.GreaterThanOrEqualTo(right);
        }

        public static Expression operator <=(Expression left, dynamic right)
        {
            return left.LesserThanOrEqualTo(right);
        }
    }
}