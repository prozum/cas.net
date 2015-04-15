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

        public abstract NotNumber Clone();

        protected virtual T MakeClone<T>() where T : NotNumber, new()
        {
            T res = new T();
            res.identifier = identifier;
            res.prefix = prefix.Clone();
            res.exponent = exponent.Clone();
            res.functionCall = functionCall;
            res.evaluator = evaluator;

            return res;
        }

        public override bool ContainsNotNumber(NotNumber other)
        {
            if (identifier == other.identifier && this.GetType() == other.GetType())
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
            else if (this is UserDefinedFunction)
            {
                return (this as UserDefinedFunction).GetValue(other).ContainsNotNumber(other);
            }
            else if (this is Symbol)
            {
                return (this as Symbol).GetValue(other).ContainsNotNumber(other);
            }

            return false;
        }
    }
}
