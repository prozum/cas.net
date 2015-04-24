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
            
            if (prefix.CompareTo(Constant.MinusOne))
            {
                res = "-" + identifier;
            }
            else if (!prefix.CompareTo(Constant.One))
            {
                res = prefix.ToString() + identifier;
            }
            else
            {
                res = identifier;
            }

            if (!exponent.CompareTo(Constant.One))
            {
                res += "^" + exponent.ToString();
            }
            
            return res;
        }

        public override Expression Evaluate()
        {
            return GetValue().Simplify().Evaluate();
        }
            
        public Expression GetValue()
        {
            var value = scope.GetVar(identifier);

            if (value == null)
                return new Error(this, "Could not get value");

            return value;
        }

        public override bool CompareTo(Expression other)
        {
            var otherSimplified = other.Simplify();

            if (otherSimplified is Symbol)
            {
                if (identifier == (otherSimplified as Symbol).identifier && prefix.CompareTo((otherSimplified as Symbol).prefix) && exponent.CompareTo((otherSimplified as Symbol).exponent))
                {
                    return true;
                }

                return GetValue().CompareTo((otherSimplified as Symbol).GetValue());
            }

            return otherSimplified.CompareTo(this.GetValue());
        }

        public override Expression Simplify()
        {
            if (prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(Constant.Zero))
            {
                return prefix;
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
//            else
//            {
//                return (this as Symbol).GetValue(other).ContainsVariable(other);
//            }

            return false;
        }

    }
}

