using System;
using System.Collections.Generic;

namespace Ast
{
    public class List : Expression
    {
        public List<Expression> elements;

        public List()
        {
            elements = new List<Expression> ();
        }


        public override Expression Evaluate()
        {
            return this;
        }

        public override string ToString()
        {
            string str = "{";

            for (int i = 0; i < elements.Count; i++) 
            {
                str += elements[i].ToString ();

                if (i < elements.Count - 1) 
                {
                    str += ',';
                }
            }

            str += "}";

            return str;
        }

        public override bool ContainsNotNumber(NotNumber other)
        {
            foreach (var item in elements)
            {
                if (item.ContainsNotNumber(other))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

