using System;
using System.Linq;
using System.Collections.Generic;

namespace Ast
{
    public enum ArgKind
    {
        Expression,
        Number,
        Symbol,
        Function,
        Equation,
        List
    }

    public abstract class Func : Variable
    {
        public List<Expression> args;

        public Func(string identifier, List<Expression> args, Scope scope)
            : base(identifier,  scope) 
        {
            this.args = args;
        }

        public override bool CompareTo(Expression other)
        {
            if (other is SysFunc)
            {
                return identifier == (other as Func).identifier && prefix.CompareTo((other as Func).prefix) && exponent.CompareTo((other as Func).exponent) && CompareArgsTo(other as Func);
            }

            if (this is UsrFunc)
            {
                return (this as UsrFunc).GetValue().CompareTo(other);
            }

            return false;
        }

        public bool CompareArgsTo(Func other)
        {
            bool res = true;

            if (args.Count == (other as Func).args.Count)
            {
                for (int i = 0; i < args.Count; i++)
                {
                    if (!args[i].CompareTo((other as Func).args[i]))
                    {
                        res = false;
                        break;
                    }
                }
            }
            else
            {
                res = false;
            }

            return res;
        }

        internal override Expression Reduce(Expression caller)
        {
            if (prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }

            return this;
        }

        protected override T MakeClone<T>()
        {
            T res = base.MakeClone<T>();
            (res as Func).args = new List<Expression>(args);

            return res;
        }

        protected T ReduceHelper<T>() where T : Func, new()
        {
            var newArgs = new List<Expression>();
            var res = new T();

            foreach (var arg in args)
            {
                newArgs.Add(arg.Reduce());
            }

            res.args = newArgs;
            res.identifier = identifier;
            res.scope = scope;
            res.prefix = prefix.Clone() as Real;
            res.exponent = exponent.Clone() as Real;

            return res;
        }
    }
}

