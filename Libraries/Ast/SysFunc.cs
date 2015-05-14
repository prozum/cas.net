using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class SysFunc : Func
    {
        public List<ArgKind> ValidArguments;

        public SysFunc(string identifier, List<Expression> args, Scope scope)
            : base(identifier, args, scope) { }

        public override string ToString ()
        {
            string str = "";

            if (prefix.CompareTo(Constant.MinusOne))
            {
                str += "-" + identifier + '[';
            }
            else if (!prefix.CompareTo(Constant.One))
            {
                str += prefix.ToString() + identifier + '[';
            }
            else
            {
                str += identifier + '[';
            }

            for (int i = 0; i < ValidArguments.Count; i++) 
            {
                str += args[i].ToString ();

                if (i < args.Count - 1) 
                {
                    str += ',';
                }
            }

            str += ']';

            if (!exponent.CompareTo(Constant.One))
            {
                str += '^' + exponent.ToString();
            }

            return str;
        }

        public bool isArgsValid()
        {
            if (args.Count != ValidArguments.Count)
                return false;

            for (int i = 0; i < args.Count; i++)
            {
                switch (ValidArguments[i])
                {
                    case ArgKind.Expression:
                        if (!(args[i] is Expression))
                            return false;
                        break;
                    case ArgKind.Number:
                        if (!(args[i] is Real))
                            return false;
                        break;
                    case ArgKind.Symbol:
                        if (!(args[i] is Symbol))
                            return false;
                        break;
                    case ArgKind.Function:
                        if (!(args[i] is Func))
                            return false;
                        break;
                    case ArgKind.Equation:
                        if (!(args[i] is Equal))
                            return false;
                        break;
                    case ArgKind.List:
                        if (!(args[i] is List))
                            return false;
                        break;
                }
            }

            return true;
        }
    }
}

