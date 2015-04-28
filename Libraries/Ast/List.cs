using System;
using System.Collections.Generic;

namespace Ast
{
    public class List : Expression
    {
        public List<Expression> items;

        const int MaxItemPrint = 10;

        public List() : this(new List<Expression> ()) {}
        public List(List<Expression> items)
        {
            this.items = items;
        }

        protected override Expression Evaluate(Expression caller)
        {
            var res = new List();

            foreach (var item in items)
            {
                res.items.Add(item.Evaluate());
            }

            return res;
        }

        public override string ToString()
        {
            string str = "[";

            for (int i = 0; i < items.Count; i++) 
            {
                if (i >= MaxItemPrint - 1)
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

