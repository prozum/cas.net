using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
    /// <summary>
    /// A Expression which has a Inverted form. Invertable Expression are used to solve equations.
    /// </summary>
    public interface IInvertable
    {
        /// <summary>
        /// Does the inverted Expression on the parameter.
        /// </summary>
        /// <remarks> 
        /// Example 1: "x + y" would be inverted to "other - y".
        /// Example 2: "Sin(x) would be inverted to "ASin(other)".
        /// </remarks> 
        Expression InvertOn(Expression other);
    }

    /// <summary>
    /// A Expression which can be negative.
    /// </summary>
    public interface INegative
    {
        /// <summary>
        /// Determins wether or not the Expression is negative.
        /// </summary>
        bool IsNegative();

        /// <summary>
        /// Returns the negative version of this Expression. 
        /// </summary>
        /// <remarks> 
        /// ToNegative should return the positive version of the Expression if it's allready negative.
        /// </remarks> 
        Expression ToNegative();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Expression
    {
        public Expression Parent;
        public Scope Scope;
        public Pos Position;

        public bool stepped = false;

        /// <summary>
        /// 
        /// </summary>
        public virtual Expression Evaluate() 
        {
            return Reduce().Evaluate(this); 
        }

        protected virtual Expression Evaluate(Expression caller)
        {
            return new Error(this, "This type cannot evaluate");
        }

        public virtual Expression GetValue()
        {
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual Expression CurrectOperator()
        {
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Expression Expand()
        {
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Expression Reduce() { return this.Reduce(this).CurrectOperator(); }
        internal virtual Expression Reduce(Expression caller)
        {
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Expression Clone()
        {
            return new Error(this, "Cannot clone");
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool CompareTo(Expression other)
        {
            return this.GetType() == other.GetType();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool ContainsVariable(Variable other)
        {
            return false;
        }

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

        public virtual Expression AddWith(Variable other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression AddWith(Operator other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression AddWith(Error other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(List other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(Scope other)
        {
            return new Error(this, "Don't support adding " + other.GetType().Name);
        }

        public virtual Expression AddWith(Text other)
        {
            return new Text(this.ToString() + other.Value);
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

        public virtual Expression SubWith(Variable other)
        {
            return this - other.Evaluate();
        }

        public virtual Expression SubWith(Operator other)
        {
            return this - other.Evaluate();
        }

        public virtual Expression SubWith(Error other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(List other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(Scope other)
        {
            return new Error(this, "Don't support subbing " + other.GetType().Name);
        }

        public virtual Expression SubWith(Text other)
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

        public virtual Expression MulWith(Variable other)
        {
            return this * other.Evaluate();
        }

        public virtual Expression MulWith(Operator other)
        {
            return this * other.Evaluate();
        }

        public virtual Expression MulWith(Error other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(List other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(Scope other)
        {
            return new Error(this, "Don't support multipying " + other.GetType().Name);
        }

        public virtual Expression MulWith(Text other)
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

        public virtual Expression DivWith(Variable other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression DivWith(Operator other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression DivWith(Error other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(List other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(Scope other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        public virtual Expression DivWith(Text other)
        {
            return new Error(this, "Don't support diving " + other.GetType().Name);
        }

        #endregion

        #region ModuloWith
        public virtual Expression ModWith(Integer other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Rational other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Irrational other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Boolean other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Complex other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Variable other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression ModWith(Operator other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression ModWith(Error other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(List other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Scope other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Text other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModWith(Expression other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
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

        public virtual Expression ExpWith(Variable other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression ExpWith(Operator other)
        {
            return this / other.Evaluate();
        }

        public virtual Expression ExpWith(Error other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(List other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(Scope other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        public virtual Expression ExpWith(Text other)
        {
            return new Error(this, "Don't support powering " + other.GetType().Name);
        }

        #endregion

        #region AndWith
        public virtual Expression AndWith(Integer other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(Rational other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(Irrational other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(Boolean other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(Complex other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(Variable other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression AndWith(Operator other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression AndWith(Error other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(List other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(Scope other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression AndWith(Text other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        #endregion

        #region AndWith
        public virtual Expression OrWith(Integer other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(Rational other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(Irrational other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(Boolean other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(Complex other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(Variable other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression OrWith(Operator other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression OrWith(Error other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(List other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(Scope other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression OrWith(Text other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
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

        public virtual Expression GreaterThan(Variable other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression GreaterThan(Operator other)
        {
            return this > other.Evaluate();
        }

        public virtual Expression GreaterThan(Error other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(List other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(Scope other)
        {
            return new Error(this, "Don't support greater than " + other.GetType().Name);
        }

        public virtual Expression GreaterThan(Text other)
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

        public virtual Expression LesserThan(Variable other)
        {
            return this < other.Evaluate();
        }

        public virtual Expression LesserThan(Operator other)
        {
            return this < other.Evaluate();
        }

        public virtual Expression LesserThan(Error other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(List other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(Scope other)
        {
            return new Error(this, "Don't support lesser than " + other.GetType().Name);
        }

        public virtual Expression LesserThan(Text other)
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

        public virtual Expression GreaterThanOrEqualTo(Variable other)
        {
            return this >= other.Evaluate();
        }

        public virtual Expression GreaterThanOrEqualTo(Operator other)
        {
            return this >= other.Evaluate();
        }

        public virtual Expression GreaterThanOrEqualTo(Error other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(List other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(Scope other)
        {
            return new Error(this, "Don't support greater than or equal to " + other.GetType().Name);
        }

        public virtual Expression GreaterThanOrEqualTo(Text other)
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

        public virtual Expression LesserThanOrEqualTo(Variable other)
        {
            return this <= other.Evaluate();
        }

        public virtual Expression LesserThanOrEqualTo(Operator other)
        {
            return this <= other.Evaluate();
        }

        public virtual Expression LesserThanOrEqualTo(Error other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(List other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(Scope other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        public virtual Expression LesserThanOrEqualTo(Text other)
        {
            return new Error(this, "Don't support lesser than or equal " + other.GetType().Name);
        }

        #endregion


        public virtual Expression Minus()
        {
            return new Error(this, "Don't support minus");
        }

        public virtual Expression Negation()
        {
            return new Error(this, "Don't support negation");
        }

        #region Binary Operator Overload
        public static Expression operator +(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.AddWith(right);
        }

        public static Expression operator -(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.SubWith(right);
        }

        public static Expression operator *(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.MulWith(right);
        }

        public static Expression operator /(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            if (right.CompareTo(Constant.Zero))
            {
                return new Error(left, "Cannot divide with 0");
            }

            return left.DivWith(right);
        }

        public static Expression operator %(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            if (right.CompareTo(Constant.Zero))
            {
                return new Error(left, "Cannot modulo with 0");
            }

            return left.ModWith(right);
        }

        public static Expression operator ^(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.ExpWith(right);
        }

        public static Expression operator &(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.AndWith(right);
        }

        public static Expression operator |(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.OrWith(right);
        }

        public static Expression operator >(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.GreaterThan(right);
        }

        public static Expression operator <(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.LesserThan(right);
        }

        public static Expression operator >=(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.GreaterThanOrEqualTo(right);
        }

        public static Expression operator <=(Expression left, dynamic right)
        {
            if (left is Error)
                return left;
            else if (right is Error)
                return right;

            return left.LesserThanOrEqualTo(right);
        }

        #endregion
    }
}