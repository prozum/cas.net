using System;
using System.Collections.Generic;

namespace Ast
{
    public class SymbolFunc : Func
    {
        public Expression expr;

        public SymbolFunc() : this(null, null, null) { }
        public SymbolFunc(string identifier, List<Expression> args, Scope scope) : base(identifier, args, scope) { }

        protected override Expression Evaluate(Expression caller)
        {
            return GetValue().Evaluate();
        }

        public Expression GetValue()
        {
            Expression @var = Scope.GetVar(Identifier);

            if (@var == null)
                return new Error(this,"has no definition");

            if (@var is SymbolFunc)
            {
                var symFuncDef = (SymbolFunc)@var;

                if (Arguments.Count != symFuncDef.Arguments.Count)
                    return new Error(Identifier + " takes " + symFuncDef.Arguments.Count.ToString() + " arguments. Not " + Arguments.Count.ToString() + ".");

                Scope = new Scope(Scope);

                for (int i = 0; i < Arguments.Count; i++)
                {
                    var arg = (Symbol)symFuncDef.Arguments[i];

                    Scope.SetVar(arg.Identifier, Arguments[i]);
                }

                expr = symFuncDef.expr;

                return expr.Evaluate();
            }

            if (@var is List)
            {
                var list = (List)@var;

                if (Arguments.Count != 1 || !(Arguments[0] is Integer))
                    return new Error(list, "Valid args: [Integer]");

                var @long = (Arguments[0] as Integer).@int;

                if (@long < 0)
                    return new Error(list, "Cannot access with negative integer");

                int @int;

                if (@long > int.MaxValue)
                    return new Error(list, "Integer is too big");
                else
                    @int = (int)@long;

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
            return MakeClone<SymbolFunc>();
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

            return GetValue();
        }

        public override string ToString()
        {
            string str = Identifier + "[";

            for (int i = 0; i < Arguments.Count; i++)
            {
                str += Arguments[i];

                if (i < Arguments.Count - 1)
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
                foreach (var item in (this as Func).Arguments)
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

