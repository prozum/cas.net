using System;
using System.Linq;
using System.Collections.Generic;

namespace Ast
{
    public enum ArgKind
    {
        Expression,
        Real,
        Variable,
        Function,
        Equation,
        List
    }

    public abstract class Func : Variable
    {
        private Scope _scope;
        public override Scope Scope
        {
            get
            {
                return _scope;
            }
            set
            {
                _scope = value;

                if (Arguments != null)
                {
                    foreach (var arg in Arguments)
                    {
                        arg.Scope = value;
                    }
                }
            }
        }

        public List<Expression> Arguments;

        public Func(string identifier, List<Expression> args, Scope scope)
            : base(identifier,  scope) 
        {
            Arguments = args;
        }

        public override bool CompareTo(Expression other)
        {
            if (other is Func)
            {
                return Identifier == (other as Func).Identifier && Prefix.CompareTo((other as Func).Prefix) && Exponent.CompareTo((other as Func).Exponent) && CompareArgsTo(other as Func);
            }

            return false;
        }

        public override bool ContainsVariable(Variable other)
        {
            foreach (var item in Arguments)
            {
                if (item.ContainsVariable(other))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CompareArgsTo(Func other)
        {
            bool res = true;

            if (Arguments.Count == (other as Func).Arguments.Count)
            {
                for (int i = 0; i < Arguments.Count; i++)
                {
                    if (!Arguments[i].CompareTo((other as Func).Arguments[i]))
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
            if (Prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (Exponent.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }

            return this;
        }

        protected override T MakeClone<T>()
        {
            T res = base.MakeClone<T>();
            (res as Func).Arguments = new List<Expression>(Arguments);

            return res;
        }

        protected T ReduceHelper<T>() where T : Func, new()
        {
            var newArgs = new List<Expression>();
            var res = new T();

            foreach (var arg in Arguments)
            {
                newArgs.Add(arg.Reduce());
            }

            res.Arguments = newArgs;
            res.Identifier = Identifier;
            res.Scope = Scope;
            res.Prefix = Prefix.Clone() as Real;
            res.Exponent = Exponent.Clone() as Real;

            return res;
        }
    }
}

