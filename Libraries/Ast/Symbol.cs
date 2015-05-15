using System;
using System.Collections.Generic;

namespace Ast
{
    public class Symbol : Variable
    {
        public Symbol() : this(null, null) { }
        public Symbol(string sym, Scope scope) : base(sym, scope) { }

        public override string ToString()
        {
            string res = "";
            
            if (Prefix.CompareTo(Constant.MinusOne))
            {
                res = "-" + Identifier;
            }
            else if (!Prefix.CompareTo(Constant.One))
            {
                res = Prefix.ToString() + Identifier;
            }
            else
            {
                res = Identifier;
            }

            if (!Exponent.CompareTo(Constant.One))
            {
                res += "^" + Exponent.ToString();
            }
            
            return res;
        }

        protected override Expression Evaluate(Expression caller)
        {
            return GetValue().Evaluate();
        }
            
        public Expression GetValue()
        {
            var value = Scope.GetVar(Identifier);

            if (value is Real)
                return Prefix * value ^ Exponent;
            else
                return value;
        }

        public override bool CompareTo(Expression other)
        {
            var otherSimplified = other.Reduce();

            if (otherSimplified is Symbol)
            {
                if (Identifier == (otherSimplified as Symbol).Identifier && Prefix.CompareTo((otherSimplified as Symbol).Prefix) && Exponent.CompareTo((otherSimplified as Symbol).Exponent))
                {
                    return true;
                }

                return GetValue().CompareTo((otherSimplified as Symbol).GetValue());
            }

            return otherSimplified.CompareTo(this.GetValue());
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
            
        public override Expression Clone()
        {
            var res = MakeClone<Symbol>();

            return res;
        }

        public override bool ContainsVariable(Variable other)
        {
            if (base.ContainsVariable(other))
            {
                return true;
            }
            else
            {
                return GetValue().ContainsVariable(other);
            }
        }

    }
}

