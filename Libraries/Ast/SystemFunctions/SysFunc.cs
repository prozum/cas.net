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
        Scope,
        Equation,
        List
    }

    public abstract class SysFunc : Scope, ICallable
    {
        public List<ArgumentType> ValidArguments;
        public string Identifier;

        public SysFunc(string identifier, Scope scope)
        {
            Identifier = identifier;
            CurScope = scope;
        }

        public static Call MakeFunction<T> (List args, Scope scope) where T : SysFunc, new()
        {
            var func = new Call(args, scope);
            func.Child = new T();
            func.Child.CurScope = scope;
            return func;
        }

        public override Expression Evaluate()
        {
            return this;
        }

        public abstract Expression Call(List args);

        public virtual Expression Reduce(List args, Scope scope)
        {
            return this;
        }

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
            bool valid = true;

            if (args.Count != ValidArguments.Count)
                valid = false;
                
            for (int i = 0; i < args.Count; i++)
            {
                if (!valid)
                    break;

                switch (ValidArguments[i])
                {
                    case ArgumentType.Expression:
                        if (args[i].Value is Error)
                            valid = false;
                        break;
                    case ArgumentType.Real:
                        if (!(args[i].Evaluate() is Real))
                            valid = false;
                        break;
                    case ArgumentType.Number:
                        if (!(args[i].Evaluate() is Number))
                            valid = false;
                        break;
                    case ArgumentType.Text:
                        if (!(args[i].Evaluate() is Text))
                            valid = false;
                        break;
                    case ArgumentType.Variable:
                        if (!(args[i] is Variable))
                            valid = false;
                        break;
                    case ArgumentType.Function:
                        if (!(args[i] is ICallable))
                            valid = false;
                        break;
                    case ArgumentType.Scope:
                        if (!(args[i].Value is Scope))
                            valid = false;
                        break;
                    case ArgumentType.Equation:
                        if (!(args[i].Value is Equal))
                            valid = false;
                        break;
                    case ArgumentType.List:
                        if (!(args[i].Evaluate() is List))
                            valid = false;
                        break;
                }
            }

            if (valid)
                return true;

            return false;
        }

        public Error GetArgumentError(List args)
        {
            return new ArgumentError(this);
        }
    }
}

