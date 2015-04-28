using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast
{
    public abstract class Variable : Expression, INegative
    {
        public string identifier;
        public Number prefix, exponent;

        protected Variable(string identifier) : this(identifier, null) { }
        protected Variable(string identifier, Scope scope)
        {
            this.identifier = identifier;
            this.scope = scope;

            this.exponent = new Integer(1);
            this.prefix = new Integer(1);
        }

        protected virtual T MakeClone<T>() where T : Variable, new()
        {
            T res = new T();
            res.identifier = identifier;
            res.prefix = prefix.Clone() as Number;
            res.exponent = exponent.Clone() as Number;
            res.scope = scope;

            return res;
        }

        public override bool ContainsVariable(Variable other)
        {
            if (identifier == other.identifier && this.GetType() == other.GetType())
            {
                return true;
            }
            else if (this is Func)
            {
                foreach (var item in (this as Func).args)
                {
                    if (item.ContainsVariable(other))
                    {
                        return true;
                    }
                }
            }
            else if (this is Symbol)
            {
                return (this as Symbol).GetValue().ContainsVariable(other);
            }

            return false;
        }

        protected Expression ReturnValue(Expression res)
        {
            if (prefix.CompareTo(Constant.Zero))
            {
                res = new Integer(0);
            }
            else
            {
                if (exponent.CompareTo(Constant.Zero))
                {
                    res = prefix.Clone();
                }
                else
                {
                    if (!exponent.CompareTo(Constant.One))
                    {
                        res = new Exp(res, exponent);
                    }

                    if (!prefix.CompareTo(Constant.One))
                    {
                        return new Mul(prefix, res);
                    }
                }
            }

            return res;
        }

        public Expression SeberateNumbers()
        {
            var thisClone = Clone() as Symbol;
            thisClone.prefix = new Integer(1);
            thisClone.exponent = new Integer(1);

            return ReturnValue(thisClone);
        }

        public bool IsNegative()
        {
            return prefix is INegative && (prefix as INegative).IsNegative();
        }

        public Expression ToNegative()
        {
            var res = Clone();
            (res as Variable).prefix = (prefix as INegative).ToNegative() as Number;

            return res;
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
