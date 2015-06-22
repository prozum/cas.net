using System.Collections.Generic;

namespace Ast
{
    public class VarFunc : Scope, ICallable
    {
        public Stack<Scope> CallStack = new Stack<Scope>();
        public string Identifier;
        public List Arguments;
        public Expression Definition;

        public VarFunc() : this(null, null, null, null) { }
        public VarFunc(string identifier, Expression definition, List args, Scope scope)
        {
            Identifier = identifier;
            Definition = definition;
            Definition.CurScope = this;
            Arguments = args;
            CurScope = scope;
        }

        public readonly int MaxFunctionRecursion = 1000;

        public override Scope CurScope 
        { 
            get 
            {
                if (CallStack.Count > 0)
                    return CallStack.Peek();
                else
                    return base.CurScope;
            }
        }

        public bool IsArgumentsValid(List args)
        {
            if (args.Count != Arguments.Count)
                return false;

            return true;
        }

        public Error GetArgumentError(List args)
        {
            return new Error(this, this.ToString() + " takes " + Arguments.Count.ToString() + " arguments. Not " + args.Count.ToString() + ".");
        }

        public override Expression Evaluate()
        {
            return this;
        }

        public Expression Call(List args)
        {
            if (CallStack.Count > MaxFunctionRecursion)
                return new Error(this, "Maximum function recursion exceeded");;
                
            var callScope = new Scope(CurScope);
            CallStack.Push(callScope);

            for (int i = 0; i < args.Count; i++)
            {
                var arg = args[i].Value;
                arg.CurScope = callScope;
                callScope.SetVar((Arguments[i] as Variable).Identifier, arg);
            }

            var res = Definition.Evaluate();
            CallStack.Pop();

            return res;
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

