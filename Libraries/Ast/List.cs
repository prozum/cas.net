using System;
using System.Collections.Generic;

namespace Ast
{
    public class List : Expression
    {
        public List<Expression> elements;

        const int MaxElementPrint = 10;

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
                if (i >= MaxElementPrint)
                {
                    str += "..." + (elements.Count - i).ToString() + "e";
                    break;
                }
                else
                {
                    str += elements[i].ToString ();

                    if (i < elements.Count - 1) 
                    {
                        str += ';';
                    }
                }
            }

            str += "}";

            return str;
        }

        public override bool ContainsVariable(Variable other)
        {
            foreach (var item in elements)
            {
                if (item.ContainsVariable(other))
                {
                    return true;
                }
            }

            return false;
        }

        public override Expression Clone()
        {
            var res = new List();

            foreach (var item in elements)
            {
                res.elements.Add(item.Clone());
            }

            return res;
        }
    }
}

