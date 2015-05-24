using System;
using System.Collections.Generic;

namespace Ast
{
    public enum ArgumentType
    {
        Expression,
        Real,
        Number,
        Text,
        Variable,
        Function,
        Equation,
        List
    }

    public abstract class SysFunc : Expression, ICallable
    {
        public List<ArgumentType> ValidArguments;
        public string Identifier;

        public SysFunc(string identifier, Scope scope)
        {
            Identifier = identifier;
            CurScope = scope;
        }

        public Call MakeFunction<T> (List args, Scope scope) where T : SysFunc, new()
        {
            var func = new Call(args, scope);
            func.Child = new T();
            func.Child.CurScope = scope;
            return func;
        }

        public abstract Expression Call(List args);

        public override string ToString ()
        {
            string str = Identifier + '[';

            for (int i = 0; i < ValidArguments.Count; i++) 
            {
                str += ValidArguments[i].ToString ();

                if (i < ValidArguments.Count - 1) 
                {
                    str += ',';
                }
            }

            return str + ']';
        }

        public bool IsArgumentsValid(List args)
        {
            if (args.Count != ValidArguments.Count)
                return false;

            for (int i = 0; i < args.Count; i++)
            {
                switch (ValidArguments[i])
                {
                    case ArgumentType.Expression:
                        if (args[i].Value is Error)
                            return false;
                        break;
                    case ArgumentType.Real:
                        if (!(args[i].Evaluate() is Real))
                            return false;
                        break;
                    case ArgumentType.Number:
                        if (!(args[i].Evaluate() is Number))
                            return false;
                        break;
                    case ArgumentType.Text:
                        if (!(args[i].Evaluate() is Text))
                            return false;
                        break;
                    case ArgumentType.Variable:
                        if (!(args[i] is Variable))
                            return false;
                        break;
                    case ArgumentType.Function:
                        if (!(args[i] is ICallable))
                            return false;
                        break;
                    case ArgumentType.Equation:
                        if (!(args[i].Value is Equal))
                            return false;
                        break;
                    case ArgumentType.List:
                        if (!(args[i].Evaluate() is List))
                            return false;
                        break;
                }
            }

            return true;
        }
    }
}

