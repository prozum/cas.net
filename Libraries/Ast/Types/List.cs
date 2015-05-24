using System;
using System.Collections.Generic;

namespace Ast
{
    public class List : Expression, ICallable
    {
        public List<Expression> Items;

        const int MaxItemPrint = 10;

        public Expression this[int i]
        {
            get { return Items[i]; }
            set { Items[i] = value; }
        }

        public int Count { get { return Items.Count; }}

        public override Scope CurScope
        {
            get { return base.CurScope; }
            set
            {
                base.CurScope = value;

                foreach (var item in Items)
                {
                    item.CurScope = value;
                }
            }
        }

        public List() : this(new List<Expression> ()) {}
        public List(List<Expression> items)
        {
            this.Items = items;
        }

        public override Expression Evaluate()
        {
            var res = new List();

            foreach (var item in Items)
            {
                res.Items.Add(item.Evaluate());
            }

            return res;
        }

        public bool IsArgumentsValid(List args)
        {
            if (args.Count != 1 || !(args[0].Evaluate() is Integer))
            {
                CurScope.Errors.Add(new ErrorData(this,"Valid args: [Integer]"));
                return false;
            }

            return true;
        }

        public Expression Call(List args)
        {
            var @long = (args[0].Evaluate() as Integer).@int;

            if (@long < 0)
                return new Error(this, "Cannot access with negative integer");

            int @int;

            if (@long > int.MaxValue)
                return new Error(this, "Integer is too big");
            else
                @int = (int)@long;

            if (@int > Count - 1)
                return new Error(this, "Cannot access item " + (@int + 1).ToString() + " in list with " + Count + " items");
            else
                return this[@int];
        }

        public override string ToString()
        {
            string str = "[";

            for (int i = 0; i < Items.Count; i++) 
            {
                if (i >= MaxItemPrint - 1)
                {
                    str += "..." + Items[Items.Count - 1].ToString();
                    break;
                }
                else
                {
                    str += Items[i].ToString ();

                    if (i < Items.Count - 1) 
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
            foreach (var item in Items)
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

            foreach (var item in Items)
            {
                res.Items.Add(item.Clone());
            }

            return res;
        }
    }
}

