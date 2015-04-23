using System;
using System.Collections.Generic;

namespace Ast
{
    public class Symbol : Variable
    {
        public UserDefinedFunction functionCall;
        public Symbol() : this(null, null) { }
        public Symbol(Symbol symbolExp) : this(symbolExp.evaluator, symbolExp.identifier) { }
        public Symbol(Evaluator evaluator, string sym) : base(sym)
        {
            this.evaluator = evaluator;
        }

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
            var otherSimplified = Evaluator.SimplifyExp(other).CurrectOperator();

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

        public override void SetFunctionCall(UserDefinedFunction functionCall)
        {
            this.functionCall = functionCall;
        }

        public override Expression Clone()
        {
            var res = MakeClone<Symbol>();
            res.functionCall = functionCall;

            return res;
        }

        public override bool ContainsVariable(Variable other)
        {
            if (base.ContainsVariable(other))
            {
                return true;
                /*
                if ((other.functionCall == null && functionCall == null) || (((other.functionCall != null && functionCall != null)) && other.functionCall.CompareTo(functionCall)))
                {
                    return true;
                }
                */
            }
            else
            {
                return (this as Symbol).GetValue(other).ContainsVariable(other);
            }

            return false;
        }

    }
}

