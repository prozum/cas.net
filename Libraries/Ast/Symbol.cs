using System;
using System.Collections.Generic;

namespace Ast
{
    public class Symbol : NotNumber
    {
        public Symbol() : this(null, null, new Integer(1), new Integer(1)) { }
        public Symbol(Evaluator evaluator, string sym) : this(evaluator, sym, new Integer(1), new Integer(1)) { }
        public Symbol(Evaluator evaluator, string sym, Number prefix, Number exponent) : base(sym, prefix, exponent)
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
            return GetValue().Evaluate();
        }

        private Expression GetValue() { return GetValue(identifier); }
        private Expression GetValue(string callerSymbol)
        {
            Expression res;

            if (this.functionCall is UserDefinedFunction)
            {

                if (functionCall.tempDefinitions.ContainsKey(identifier))
                {
                    functionCall.tempDefinitions.TryGetValue(identifier, out res);

                    if (res is Symbol)
                    {
                        if ((res as Symbol).identifier == callerSymbol)
                        {
                            return new Error("Symbol> Could not get value of: " + callerSymbol);
                        }

                        return (res as Symbol).GetValue(callerSymbol);
                    }

                    return new Mul(prefix, new Exp(res, exponent));
                }
            }
            else
            {
                if (evaluator.variableDefinitions.ContainsKey(identifier))
                {
                    evaluator.variableDefinitions.TryGetValue(identifier, out res);

                    if (res is Symbol)
                    {
                        if ((res as Symbol).identifier == callerSymbol)
                        {
                            return new Error("Symbol> Could not get value of: " + callerSymbol);
                        }

                        return (res as Symbol).GetValue(callerSymbol);
                    }

                    return new Mul(prefix, new Exp(res, exponent));
                }
            }

            return new Error("Symbol> Could not get Symbol value");
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
                return new Integer(1);
            }

            return base.Simplify();
        }
    }
}

