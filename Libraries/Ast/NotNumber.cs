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
    }
}
