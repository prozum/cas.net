using System;
using System.Collections.Generic;

namespace Ast
{
    public class Symbol : Variable
    {
        public Symbol() : this(null, null) { }
        //public Symbol(Symbol symbolExp) : this(symbolExp.evaluator, symbolExp.identifier) { }
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
            return Evaluator.SimplifyExp(GetValue()).Evaluate();
        }
            
        public Expression GetValue()
        {
            var value = scope.GetVar(identifier);

            if (value == null)
                return new Error(this, "Could not get value");

            return value;
//
//            if (this.functionCall is UserDefinedFunction)
//            {
//                if (functionCall.locals.ContainsKey(identifier))
//                {
//                    functionCall.locals.TryGetValue(identifier, out res);
//
//                    if (res.ContainsVariable(other) && res.Evaluate() is Error)
//                    {
//                        return new Error(this, "Could not get value of: " + other.identifier);
//                    }
//
//                    return ReturnValue(res.Clone());
//                }
//            }
//            else
//            {
//                if (evaluator.globals.ContainsKey(identifier))
//                {
//                    evaluator.globals.TryGetValue(identifier, out res);
//
//                    if (res.ContainsVariable(other))
//                    {
//                        return new Error(this, "Could not get value of: " + other.identifier);
//                    }
//
//                    return ReturnValue(res.Clone());
//                }
//            }
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
            if (prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(Constant.Zero))
            {
                return prefix;
            }

            return base.Simplify();
        }

//        public override void SetFunctionCall(UserDefinedFunction functionCall)
//        {
//            this.functionCall = functionCall;
//        }

        public override Expression Clone()
        {
            return MakeClone<Symbol>();
        }
    }
}

