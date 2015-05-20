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

            if (Prefix.CompareTo(Constant.MinusOne))
            {
                str += "-" + Identifier + '[';
            }
            else if (!Prefix.CompareTo(Constant.One))
            {
                str += Prefix.ToString() + Identifier + '[';
            }
            else
            {
                str += Identifier + '[';
            }

            for (int i = 0; i < ValidArguments.Count; i++) 
            {
                str += Arguments[i].ToString ();

                if (i < Arguments.Count - 1) 
                {
                    str += ',';
                }
            }

            str += ']';

            if (!Exponent.CompareTo(Constant.One))
            {
                str += '^' + Exponent.ToString();
            }

            return str;
        }

        public bool IsArgumentsValid()
        {
            if (Arguments.Count != ValidArguments.Count)
                return false;

            for (int i = 0; i < Arguments.Count; i++)
            {
                switch (ValidArguments[i])
                {
                    case ArgKind.Expression:
                        if (!(Arguments[i].Value is Expression))
                            return false;
                        break;
                    case ArgKind.Real:
                        if (!(Arguments[i].Evaluate() is Real))
                            return false;
                        break;
                    case ArgKind.Variable:
                        if (!(Arguments[i] is Variable))
                            return false;
                        break;
                    case ArgKind.Function:
                        if (!(Arguments[i] is Func))
                            return false;
                        break;
                    case ArgKind.Equation:
                        if (!(Arguments[i].Value is Equal))
                            return false;
                        break;
                    case ArgKind.List:
                        if (!(Arguments[i].Value is List))
                            return false;
                        break;
                }
            }

            return true;
        }
    }
}

