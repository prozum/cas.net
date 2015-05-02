using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
    public interface IInvertable
    {
        Expression Inverted(Expression other);
    }

    public interface INegative
    {
        bool IsNegative();
        Expression ToNegative();
    }

    public abstract class Expression
    {
        public BinaryOperator parent;
        public Scope scope;
        public Pos pos;

        public bool stepped = false;

        public virtual Expression Evaluate() 
        {
            return Reduce().Evaluate(this); 
        }
        protected virtual Expression Evaluate(Expression caller)
        {
            return new Error(this, "This type cannot evaluate");
        }

        public virtual EvalData Step()
        {
            if (stepped)
                return new DoneData();
            stepped =  !stepped;

            var res = Evaluate();

            if (res is Error)
                return new ErrorData(res as Error);
            else
                return new DebugData("Evaluate: " + this.ToString() + " = ", res);
        }

        public virtual Expression CurrectOperator()
        {
            return this;
        }

        public virtual Expression Expand()
        {
            return this;
        }

        public virtual Expression Reduce() { return this.Reduce(this).CurrectOperator(); }
        internal virtual Expression Reduce(Expression caller)
        {
            return this;
        }

        public virtual Expression Clone()
        {
            return new Error(this, "Cannot clone");
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

        public virtual Expression AddWith(Message other)
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

        public virtual Expression SubWith(Variable other)
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

        public virtual Expression MulWith(Message other)
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

        public virtual Expression DivWith(Message other)
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

        public virtual Expression ExpWith(Message other)
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

        public virtual Expression GreaterThan(Message other)
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

        public virtual Expression LesserThan(Message other)
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

        public virtual Expression GreaterThanOrEqualTo(Message other)
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

        public virtual Expression LesserThanOrEqualTo(Message other)
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

        #region ModuloWith
        public virtual Expression ModuloWith(Integer other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(Rational other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(Irrational other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(Boolean other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(Complex other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(Variable other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression ModuloWith(Operator other)
        {
            return this + other.Evaluate();
        }

        public virtual Expression ModuloWith(Message other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(List other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(Scope other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
        }

        public virtual Expression ModuloWith(Text other)
        {
            return new Error(this, "Don't support modulo " + other.GetType().Name);
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
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.AddWith(right);
        }

        public static Expression operator -(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.SubWith(right);
        }

        public static Expression operator *(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.MulWith(right);
        }

        public static Expression operator /(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            if (right.CompareTo(Constant.Zero))
            {
                return new Error(left, "Cannot be divided by 0");
            }

            return left.DivWith(right);
        }

        public static Expression operator ^(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.ExpWith(right);
        }

        public static Expression operator >(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.GreaterThan(right);
        }

        public static Expression operator <(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.LesserThan(right);
        }

        public static Expression operator >=(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.GreaterThanOrEqualTo(right);
        }

        public static Expression operator <=(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.LesserThanOrEqualTo(right);
        }

        public static Expression operator %(Expression left, dynamic right)
        {
            if (left is Message)
                return left;
            else if (right is Message)
                return right;

            return left.ModuloWith(right);
        }
        #endregion
    }
}