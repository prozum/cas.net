using System;
using System.Collections.Generic;

namespace Ast
{
    public class UsrFunc : Func
    {
        public Expression expr;

        public UsrFunc() : this(null, null, null) { }
        public UsrFunc(string identifier, List<Expression> args, Scope scope) : base(identifier, args, scope) { }

        protected override Expression Evaluate(Expression caller)
        {
            return GetValue().Evaluate();
        }

        public Expression GetValue()
        {
            Expression @var;

            @var = scope.GetVar(identifier);

            if (@var == null)
                return new Error(identifier + "> has no definition");

            if (@var is UsrFunc)
            {
                var usrFuncDef = (UsrFunc)@var;

                if (args.Count != usrFuncDef.args.Count)
                    return new Error(identifier + " takes " + usrFuncDef.args.Count.ToString() + " arguments. Not " + args.Count.ToString() + ".");

                scope = new Scope(scope);

                for (int i = 0; i < args.Count; i++)
                {
                    var arg = (Symbol)usrFuncDef.args[i];

                    scope.SetVar(arg.identifier, args[i]);
                }

                expr = usrFuncDef.expr;

                return expr.Evaluate();
            }

            if (@var is List)
            {
                var list = (List)@var;

                if (args.Count != 1 || !(args[0] is Integer))
                    return new Error(list, "Valid args: [Integer]");

                var @long = (args[0] as Integer).value;
                int @int;

                if (@long > int.MaxValue)
                    return new Error(list, "Integer is too big");
                else
                    @int = (int)@long;

                if (@int < 0)
                    return new Error(list, "Cannot access with negative integer");

                if (@int > list.items.Count - 1)
                    return new Error(list, "Cannot access item " + (@int+1).ToString() + " in list with " + list.items.Count + " items");
                else
                    return list.items[@int];

            }

            return new Error(this, "Variable is not a function or list");


            //            if (def.ContainsVariable(this))
            //            {
            //                return new Error(this, "Could not get value of: " + this.identifier);
            //            }
        }

        public override Expression Clone()
        {
            return MakeClone<UsrFunc>();
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

            return GetValue();
        }

        public override string ToString()
        {
            string str = identifier + "[";

            for (int i = 0; i < args.Count; i++)
            {
                str += args[i];

                if (i < args.Count - 1)
                    str += ",";
            }
            str += "]";

            return str;
        }

        public override bool ContainsVariable(Variable other)
        {
            if (base.ContainsVariable(other))
            {
                return true;
            }
            else
            {
                foreach (var item in (this as Func).args)
                {
                    if (item.ContainsVariable(other))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

