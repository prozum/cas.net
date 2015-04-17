using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast
{
    public abstract class NotNumber : Expression
    {
        public string identifier;
        public Number prefix, exponent;

        public NotNumber(string identifier) : this(identifier, new Integer(1), new Integer(1)) { }
        public NotNumber(string identifier, Number prefix, Number exponent)
        {
            this.exponent = exponent;
            this.prefix = prefix;
            this.identifier = identifier;
        }

        protected virtual T MakeClone<T>() where T : NotNumber, new()
        {
            T res = new T();
            res.identifier = identifier;
            res.prefix = prefix.Clone() as Number;
            res.exponent = exponent.Clone() as Number;
            res.functionCall = functionCall;
            res.evaluator = evaluator;

            return res;
        }

        public override bool ContainsNotNumber(NotNumber other)
        {
            if (identifier == other.identifier && this.GetType() == other.GetType() && ((other.functionCall == null && functionCall == null) || ((other.functionCall != null && functionCall != null) && other.functionCall.CompareTo(functionCall))))
            {
                return true;
            }
            else if (this is Function)
            {
                foreach (var item in (this as Function).args)
	            {
		            if (item.ContainsNotNumber(other))
                    {
                        return true;
                    }
	            }
            }
            else if (this is Symbol)
            {
                return (this as Symbol).GetValue(other).ContainsNotNumber(other);
            }

            return false;
        }

        protected Expression ReturnValue(Expression res)
        {
            if (prefix.CompareTo(new Integer(0)))
            {
                res = new Integer(0);
            }
            else
            {
                if (exponent.CompareTo(new Integer(0)))
                {
                    res = prefix.Clone();
                }
                else
                {
                    if (!exponent.CompareTo(new Integer(1)))
                    {
                        res = new Exp(res, exponent);
                    }

                    if (!prefix.CompareTo(new Integer(1)))
                    {
                        return new Mul(prefix, res);
                    }
                }
            }

            return res;
        }

        public override Expression Expand()
        {
            Expression res;
            var symbol = Clone();

            (symbol as NotNumber).prefix = new Integer(1);
            (symbol as NotNumber).exponent = new Integer(1);

            if (!(symbol as NotNumber).exponent.CompareTo(new Integer(1)))
            {
                res = new Exp(symbol, exponent);
            } 
            else
	        {
                res = symbol;
	        }

            if (!(symbol as NotNumber).prefix.CompareTo(new Integer(1)))
            {
                res = new Mul(prefix, res);
            }

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
