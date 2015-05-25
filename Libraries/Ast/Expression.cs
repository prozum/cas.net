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
        Expression InvertOn(Expression other);
    }

    /// <summary>
    /// A Expression which can be negative.
    /// </summary>
    public interface INegative
    {
        bool IsNegative();
        Expression ToNegative();
    }

    /// <summary>
    /// Expression is a expression which must have a return value.
    /// </summary>
    public abstract class Expression
    {
        public virtual Scope CurScope { get; set; }

        public Expression Parent;
        public Pos Position;

        Expression _reducedForm = null;

        public virtual Expression Value
        {
            get { return this; }
            set { throw new Exception("Cannot set value on " + this.GetType().Name); }
        }

        public Expression ReduceEvaluate()
        {
            if (_reducedForm == null)
                _reducedForm = ReduceCurrectOp();

            return _reducedForm.Evaluate();
        }

        public virtual Expression Evaluate()
        {
            CurScope.Errors.Add(new ErrorData(this, "This type cannot evaluate"));
            return Constant.Null;
        }

        internal virtual Expression CurrectOperator()
        {
            return this;
        }

        public virtual Expression Expand()
        {
            return this;
        }

        public Expression ReduceCurrectOp()
        {
            return Reduce().CurrectOperator();
        }

        public virtual Expression Reduce()
        {
            return this;
        }

        public virtual Expression Clone()
        {
            CurScope.Errors.Add(new ErrorData(this, "Cannot clone"));
            return Constant.Null;
        }

        public virtual bool CompareTo(Expression other)
        {
            return this.GetType() == other.GetType();
        }

        public virtual bool ContainsVariable(Variable other)
        {
            return false;
        }

        #region AddWith
        public virtual Expression AddWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support adding " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AddWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support adding " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AddWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support adding " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AddWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support adding " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AddWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support adding " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AddWith(Variable other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression AddWith(Scope other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression AddWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support adding " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AddWith(Text other)
        {
            return new Text(this.ToString() + other.Value);
        }

        public virtual Expression AddWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support adding " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region SubWith
        public virtual Expression SubWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression SubWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression SubWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression SubWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression SubWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression SubWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression SubWith(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression SubWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support subbing " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region MulWith
        public virtual Expression MulWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression MulWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression MulWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression MulWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression MulWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression MulWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression MulWith(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression MulWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support multipying " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region DivWith
        public virtual Expression DivWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression DivWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression DivWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression DivWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression DivWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression DivWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression DivWith(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression DivWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support diving " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region ModWith
        public virtual Expression ModWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ModWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ModWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ModWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ModWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ModWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ModWith(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ModWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support modulo " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region ExpWith
        public virtual Expression ExpWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ExpWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ExpWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ExpWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ExpWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ExpWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ExpWith(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression ExpWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support powering " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region AndWith
        public virtual Expression AndWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AndWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AndWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AndWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AndWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AndWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AndWith(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression AndWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support and with " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region OrWith
        public virtual Expression OrWith(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression OrWith(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression OrWith(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression OrWith(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression OrWith(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression OrWith(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression OrWith(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression OrWith(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support or with " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region GreaterThan
        public virtual Expression GreaterThan(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThan(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThan(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThan(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThan(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThan(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThan(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThan(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region LesserThan
        public virtual Expression LesserThan(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThan(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThan(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThan(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThan(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThan(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThan(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThan(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region GreaterThanOrEqualTo
        public virtual Expression GreaterThanOrEqualTo(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThanOrEqualTo(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThanOrEqualTo(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThanOrEqualTo(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThanOrEqualTo(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThanOrEqualTo(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThanOrEqualTo(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression GreaterThanOrEqualTo(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support greater than or equal to " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion

        #region LesserThanOrEqualTo
        public virtual Expression LesserThanOrEqualTo(Integer other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThanOrEqualTo(Rational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThanOrEqualTo(Irrational other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThanOrEqualTo(Boolean other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThanOrEqualTo(Complex other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThanOrEqualTo(List other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThanOrEqualTo(Text other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        public virtual Expression LesserThanOrEqualTo(Null other)
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support lesser than or equal " + other.GetType().Name));
            return Constant.Null;
        }

        #endregion


        public virtual Expression Minus()
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support minus"));
            return Constant.Null;
        }

        public virtual Expression Negation()
        {
            CurScope.Errors.Add(new ErrorData(this, "Don't support negation"));
            return Constant.Null;
        }

        #region Binary Operator Overload
        public static Expression operator +(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.AddWith(right);
        }

        public static Expression operator -(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.SubWith(right);
        }

        public static Expression operator *(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.MulWith(right);
        }

        public static Expression operator /(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            if (right.CompareTo(Constant.Zero))
            {
                left.CurScope.Errors.Add(new ErrorData(left, "Cannot divide with 0"));
                return Constant.Null;
            }

            return left.DivWith(right);
        }

        public static Expression operator %(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();


            if (right.CompareTo(Constant.Zero))
            {
                left.CurScope.Errors.Add(new ErrorData(left, "Cannot modulo with 0"));
                return Constant.Null;
            }

            return left.ModWith(right);
        }

        public static Expression operator ^(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.ExpWith(right);
        }

        public static Expression operator &(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.AndWith(right);
        }

        public static Expression operator |(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.OrWith(right);
        }

        public static Expression operator >(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.GreaterThan(right);
        }

        public static Expression operator <(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.LesserThan(right);
        }

        public static Expression operator >=(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.GreaterThanOrEqualTo(right);
        }

        public static Expression operator <=(Expression left, dynamic right)
        {
            left = left.Evaluate();
            right = right.Evaluate();

            return left.LesserThanOrEqualTo(right);
        }

        #endregion
    }
}