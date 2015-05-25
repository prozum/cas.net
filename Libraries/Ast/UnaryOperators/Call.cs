using System;

namespace Ast
{
    public class Call : PostfixOperator
    {
        public List Arguments;

        public Call(List args, Scope scope) 
        {
            Arguments = args;
            CurScope = scope;
        }

        public override Scope CurScope
        {
            get { return base.CurScope; }
            set
            {
                base.CurScope = value;
                if (Arguments != null)
                {
                    Arguments.CurScope = value;
                }
            }
        }

        public override Expression Evaluate()
        {
            var val = Child.Value;

            if (val is Call)
                val = val.Evaluate().Value;

            if (CurScope.Error)
                return new Null();

            if (!(val is ICallable))
                return new Error(val, "is not callable");

            if (!(val as ICallable).IsArgumentsValid(Arguments))
                return new Null();


            return (val as ICallable).Call(Arguments);
        }

        public override Expression Reduce()
        {
            var res = new Call(Arguments, CurScope);

            if (Child is Variable)
            {
                res.Child = new Variable();
                res.Child.Value = (Child as Variable).Reduce(Arguments, CurScope);
            }
            else
                res.Child = Child.Reduce();

            return res;
        }

        public override string ToString()
        {
            return Child.ToString() + Arguments.ToString();
        }

        public override bool ContainsVariable(Variable other)
        {
            foreach (var arg in Arguments.Items)
            {
                if (!arg.ContainsVariable(other))
                    return false;

            }

            return true;
        }
    }
}

