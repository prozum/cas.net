using System;
using System.Collections.Generic;

namespace Ast
{
    public class List : Expression
    {
        public List<Expression> items;

        const int MaxElementPrint = 10;

        public List()
        {
            items = new List<Expression> ();
        }

        protected override Expression Evaluate(Expression caller)
        {
            return this;
        }

        public override string ToString()
        {
            string str = "[";

            for (int i = 0; i < items.Count; i++) 
            {
                if (i >= MaxElementPrint - 1)
                {
                    str += "..." + items[items.Count - 1].ToString();
                    break;
                }
                else
                {
                    str += items[i].ToString ();

                    if (i < items.Count - 1) 
                    {
                        str += ',';
                    }
                }
            }

            str += "]";

            return str;
        }

        public override bool ContainsVariable(Variable other)
        {
            foreach (var item in items)
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

            foreach (var item in items)
            {
                res.items.Add(item.Clone());
            }

            return res;
        }
    }
}

