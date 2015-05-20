using System;
using System.Collections.Generic;

namespace Ast
{
    public class CustomFunc : Func
    {
        public CustomFunc() : this(null, null, null) { }
        public CustomFunc(string identifier, List<Expression> args, Scope scope) : base(identifier, args, scope) { }

        public override Expression Evaluate() 
        {
            return Evaluate(this); 
        }

        internal override Expression Evaluate(Expression caller)
        {
            return Value.Evaluate();
        }


        private Expression _value;
        public override Expression Value
        {
            get
            {
                if (Definition)
                    return _value;

                var res = Scope.GetVar(Identifier);

                Variable @var;
                if (res is Variable)
                    @var = res as Variable;
                else if (res is Error)
                    return res;
                else
                    return new Error(this, "Variable is not a function or list");

                if (@var is CustomFunc)
                {
                    var customDef = (CustomFunc)@var;

                    if (Arguments.Count != customDef.Arguments.Count)
                        return new Error(Identifier + " takes " + customDef.Arguments.Count.ToString() + " arguments. Not " + Arguments.Count.ToString() + ".");

                    for (int i = 0; i < Arguments.Count; i++)
                    {
                        var arg = (Variable)customDef.Arguments[i];

                        customDef.SetVar(arg.Identifier, Arguments[i].Value);
                    }

                    return customDef;
                }

                if (@var.Value is List)
                {
                    var list = (List)@var.Value;

                    if (Arguments.Count != 1 || !(Arguments[0].Evaluate() is Integer))
                        return new Error(list, "Valid args: [Integer]");

                    var @long = (Arguments[0].Evaluate() as Integer).@int;

                    if (@long < 0)
                        return new Error(list, "Cannot access with negative integer");

                    int @int;

                    if (@long > int.MaxValue)
                        return new Error(list, "Integer is too big");
                    else
                        @int = (int)@long;

                    if (@int > list.items.Count - 1)
                        return new Error(list, "Cannot access item " + (@int + 1).ToString() + " in list with " + list.items.Count + " items");
                    else
                        return list.items[@int];
                }


                return new Error(this, "Variable is not a function or list");
            }

            set
            {
                Definition = true;
                _value = value;
            }
        }

        public override Expression Clone()
        {
            return MakeClone<CustomFunc>();
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

            return Value;
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

