using System;
using System.Collections.Generic;

namespace Ast
{
    public class Symbol : Expression
    {
        public Number prefix, exponent;
        public string symbol;

        public Symbol() : this(null, null, new Integer(1), new Integer(1)) { }
        public Symbol(Evaluator evaluator, string sym) : this(evaluator, sym, new Integer(1), new Integer(1)) { }
        public Symbol(Evaluator evaluator, string sym, Number prefix, Number exponent)
        {
            this.exponent = exponent;
            this.prefix = prefix;
            this.symbol = sym;
            this.evaluator = evaluator;
        }

        public override string ToString()
        {
            string res = "";

            if (((prefix is Integer) && (prefix as Integer).value != 1) || ((prefix is Rational) && (prefix as Rational).value.value != 1) || ((prefix is Irrational) && (prefix as Irrational).value != 1))
            {
                res = prefix.ToString() + symbol;
            }
            else
            {
                res = symbol;
            }

            if (((exponent is Integer) && (exponent as Integer).value != 1) || ((exponent is Rational) && (exponent as Rational).value.value != 1) || ((exponent is Irrational) && (exponent as Irrational).value != 1))
            {
                res += "^" + exponent.ToString();
            }
            
            return res;
        }

        public override Expression Evaluate()
        {
            Expression res;

            if (this.functionCall is UserDefinedFunction)
            {
                if (functionCall.tempDefinitions.ContainsKey(symbol))
                {
                    functionCall.tempDefinitions.TryGetValue(symbol, out res);
                    return new Mul(prefix, new Exp(res, exponent)).Evaluate();
                }
            }
            else
            {
                if (evaluator.variableDefinitions.ContainsKey(symbol))
                {
                    evaluator.variableDefinitions.TryGetValue(symbol, out res);
                    return new Mul(prefix, new Exp(res, exponent)).Evaluate();
                }
            }

            return new Error("Could not evaluate: " + symbol);
        }

        public override bool CompareTo(Expression other)
        {
            Expression thisEvaluated, otherEvaluated;
            var sameType = base.CompareTo(other);

            if (!((thisEvaluated = this.Evaluate()) is Error || (otherEvaluated = other.Evaluate()) is Error))
            {
                return (new BooleanEqual(this.Evaluate(), other.Evaluate()).Evaluate() as Boolean).value;
            }

            if (sameType)
            {
                if (symbol == (other as Symbol).symbol && prefix.CompareTo((other as Symbol).prefix) && exponent.CompareTo((other as Symbol).exponent))
                {
                    return true;
                }

                return false;
            }

            return false;
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

