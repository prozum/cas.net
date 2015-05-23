using System;
using System.Collections.Generic;

namespace Ast
{
    public class VarFunc : Func
    {
        public VarFunc() : this(null, null, null) { }
        public VarFunc(string identifier, List<Expression> args, Scope scope) : base(identifier, args, scope) { }

        public readonly int MaxFunctionRecursion = 5;

        public override Scope CurScope 
        { 
            get 
            {
                if (CallStack == null)
                    return base.CurScope;
                return CallStack.Peek(); 
            }
        }
        public Stack<Scope> CallStack;

        public override Expression Evaluate() 
        {
            return Evaluate(this); 
        }

        internal override Expression Evaluate(Expression caller)
        {
            var val = Value;

            if (val is Error)
                return val;

            if (val is VarFunc)
            {
                var def = (VarFunc)val;

                if (def.Definition)
                {
                    if (def.CallStack.Count > MaxFunctionRecursion)
                        return new Error(this, "Maximum function recursion exceeded");


                    if (Arguments.Count != def.Arguments.Count)
                        return new Error(Identifier + " takes " + def.Arguments.Count.ToString() + " arguments. Not " + Arguments.Count.ToString() + ".");

                    var callScope = new Scope(CurScope);
                    def.CallStack.Push(callScope);


                    for (int i = 0; i < Arguments.Count; i++)
                    {
                        var arg = (Variable)def.Arguments[i];
                        callScope.SetVar(arg.Identifier, Arguments[i].Evaluate());
                    }

                    var res = def.Evaluate();
                    def.CallStack.Pop();
                    return res;
                }
            }

            if (val is List)
                return val;

            //return val.Evaluate();
            throw new Exception("Not good");
        }

        public override Expression Value
        {
            get
            {
                if (Definition)
                    return _value;

                var @var = CurScope.GetVar(Identifier);

                if (@var is Error)
                    return @var;

                if (@var is VarFunc)
                    return @var;
                    
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
                CallStack = new Stack<Scope>();
                Definition = true;
                _value = value;
            }
        }

        public override Expression Clone()
        {
            return MakeClone<VarFunc>();
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

