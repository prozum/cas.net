using System;
using System.Collections.Generic;

namespace Ast
{
    public class VarFunc : Expression, ICallable
    {
        public string Identifier;
        public List Arguments;
        public Expression Definition;

        public VarFunc() : this(null, null) { }
        public VarFunc(string identifier, Scope scope)
        {
            Identifier = identifier;
            CurScope = scope;
        }

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

        public bool IsArgumentsValid(List args)
        {
            if (args.Count != Arguments.Count)
            {
                CurScope.Errors.Add(new ErrorData(this, Identifier + " takes " + Arguments.Count.ToString() + " arguments. Not " + args.Count.ToString() + "."));
                return false;
            }

            return true;
        }

        public Expression Call(List args)
        {
            if (CallStack.Count > MaxFunctionRecursion)
                return new Error(this, "Maximum function recursion exceeded");;

            var callScope = new Scope(CurScope);
            CallStack.Push(callScope);

            for (int i = 0; i < args.Count; i++)
            {
                var arg = (Variable)args[i];
                callScope.SetVar(arg.Identifier, args[i].Evaluate());
            }

            var res = Definition.Evaluate();
            CallStack.Pop();

            return res;
        }

//        public override Expression Value
//        {
//            get
//            {
//                if (Definition)
//                    return _value;
//
//                var @var = CurScope.GetVar(Identifier);
//
//                if (@var is Error)
//                    return @var;
//
//                if (@var is VarFunc)
//                {
//                    return @var;
//                }
//
//                return new Error(this, "Variable is not a function or list");
//            }
//
//            set
//            {
//                CallStack = new Stack<Scope>();
//                Definition = true;
//                _value = value;
//            }
//        }

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

//        public override bool ContainsVariable(Variable other)
//        {
//            if (base.ContainsVariable(other))
//            {
//                return true;
//            }
//            else
//            {
//                foreach (var item in (this as Func).Arguments)
//                {
//                    if (item.ContainsVariable(other))
//                    {
//                        return true;
//                    }
//                }
//            }
//
//            return false;
//        }
    }
}

