using System;
using System.Collections.Generic;

namespace Ast
{
    public class Symbol : Variable
    {
        public Symbol() : this(null, null) { }
        public Symbol(Symbol symbolExp) : this(symbolExp.evaluator, symbolExp.identifier) { }
        public Symbol(Evaluator evaluator, string sym) : base(sym)
        {
            this.evaluator = evaluator;
        }

        public override string ToString()
        {
            string res = "";
            
             if (prefix.CompareTo(new Integer(-1)))
            {
                res = "-" + identifier;
            }
            else if (!prefix.CompareTo(new Integer(1)))
            {
                res = prefix.ToString() + identifier;
            }
            else
            {
                res = identifier;
            }

            if (!exponent.CompareTo(new Integer(1)))
            {
                res += "^" + exponent.ToString();
            }
            
            return res;
        }

        public override Expression Evaluate()
        {
            return Evaluator.SimplifyExp(GetValue()).Evaluate();
        }

        public Expression GetValue() { return GetValue(this); }
        public Expression GetValue(Variable other)
        {
            Expression res;

            if (this.functionCall is UserDefinedFunction)
            {
                if (functionCall.tempDefinitions.ContainsKey(identifier))
                {
                    functionCall.tempDefinitions.TryGetValue(identifier, out res);

                    if (res.ContainsVariable(other) && res.Evaluate() is Error)
                    {
                        return new Error(this, "Could not get value of: " + other.identifier);
                    }

                    return ReturnValue(res.Clone());
                }
            }
            else
            {
                if (evaluator.variableDefinitions.ContainsKey(identifier))
                {
                    evaluator.variableDefinitions.TryGetValue(identifier, out res);

                    if (res.ContainsVariable(other))
                    {
                        return new Error(this, "Could not get value of: " + other.identifier);
                    }

                    return ReturnValue(res.Clone());
                }
            }

            return new Error(this, "Could not get Symbol value");
        }

        public override bool CompareTo(Expression other)
        {
            if (other is Symbol)
            {
                if (identifier == (other as Symbol).identifier && prefix.CompareTo((other as Symbol).prefix) && exponent.CompareTo((other as Symbol).exponent))
                {
                    return true;
                }

                return GetValue().CompareTo((other as Symbol).GetValue());
            }

            return other.CompareTo(this.GetValue());
        }

        public override Expression Simplify()
        {
            if (prefix.CompareTo(new Integer(0)))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(new Integer(0)))
            {
                return prefix;
            }

            return base.Simplify();
        }

        public override Expression Clone()
        {
            return MakeClone<Symbol>();
        }
    }
}

