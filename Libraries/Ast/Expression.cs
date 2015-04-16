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
            return new Error(this, "Don't support adding " + other.GetType().Name);
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
            return new Error(this, "Don't support adding " + other.GetType().Name);
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
            return new Error(this, "Don't support adding " + other.GetType().Name);
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
            return new Error(this, "Don't support adding " + other.GetType().Name);
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
    }
}