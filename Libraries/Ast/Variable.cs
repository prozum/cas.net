using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast
{
    public class Variable : Scope, INegative
    {
        public string Identifier;
        public Real Prefix, Exponent;

        public Variable() : this(null, null) { }
        public Variable(string identifier, Scope scope)
        {
            Identifier = identifier;
            Scope = scope;
            SideEffects = scope.SideEffects;

            Exponent = new Integer(1);
            Prefix = new Integer(1);
        }

        public override string ToString()
        {
            string res;

            if (Prefix.CompareTo(Constant.MinusOne))
            {
                res = "-" + Identifier.ToString();
            }
            else if (!Prefix.CompareTo(Constant.One))
            {
                res = Prefix.ToString() + Identifier.ToString();
            }
            else
            {
                res = Identifier.ToString();
            }

            if (!Exponent.CompareTo(Constant.One))
            {
                res += "^" + Exponent.ToString();
            }

            return res;
        }

        protected virtual T MakeClone<T>() where T : Variable, new()
        {
            T res = new T();
            res.Identifier = Identifier;
            res.Prefix = Prefix.Clone() as Real;
            res.Exponent = Exponent.Clone() as Real;
            res.Scope = Scope;
            res.Position = Position;

            return res;
        }

        public override Expression Clone()
        {
            return MakeClone<Variable>();
        }

        public override Expression Evaluate() 
        {
            return Reduce().Evaluate(this); 
        }

        internal override Expression Evaluate(Expression caller)
        {
            return Value;
        }

        protected bool Definition = false;
        private Expression _value = null;
        public override Expression Value
        {
            get
            {
                if (Definition)
                    return _value;

                var value = Scope.GetVar(Identifier).Value;

                if (value is Real)
                    value = Prefix * value ^ Exponent;

                return value;
            }

            set
            {
                Definition = true;
                _value = value;
            }
        }

        public override bool CompareTo(Expression other)
        {
            var otherSimplified = other.Reduce();

            if (otherSimplified is Variable)
            {
                if (Identifier == (otherSimplified as Variable).Identifier && Prefix.CompareTo((otherSimplified as Variable).Prefix) && Exponent.CompareTo((otherSimplified as Variable).Exponent))
                {
                    return true;
                }

                return Value.CompareTo(otherSimplified.Value);
            }

            return otherSimplified.CompareTo(this.Value);
        }

        internal override Expression Reduce(Expression caller)
        {
            if (Prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (Exponent.CompareTo(Constant.Zero))
            {
                return Prefix;
            }

            return this;
        }

        public override bool ContainsVariable(Variable other)
        {
            if (Identifier == other.Identifier && this.GetType() == other.GetType())
                return true;
            else
                return Value.ContainsVariable(other);
        }

        public Expression ReturnValue(Expression res)
        {
            if (Prefix.CompareTo(Constant.Zero))
            {
                res = new Integer(0);
            }
            else
            {
                if (Exponent.CompareTo(Constant.Zero))
                {
                    res = Prefix.Clone();
                }
                else
                {
                    if (!Exponent.CompareTo(Constant.One))
                    {
                        res = new Exp(res, Exponent);
                    }

                    if (!Prefix.CompareTo(Constant.One))
                    {
                        return new Mul(Prefix, res);
                    }
                }
            }

            return res;
        }

        public Expression SeberateNumbers()
        {
            var thisClone = Clone() as Variable;
            thisClone.Prefix = new Integer(1);
            thisClone.Exponent = new Integer(1);

            return ReturnValue(thisClone);
        }

        public bool IsNegative()
        {
            return Prefix is INegative && (Prefix as INegative).IsNegative();
        }

        public Expression ToNegative()
        {
            var res = Clone();
            (res as Variable).Prefix = (Prefix as INegative).ToNegative() as Real;

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
