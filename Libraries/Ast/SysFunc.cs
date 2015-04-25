using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class SysFunc : Func
    {
        public List<ArgKind> validArgs;

        public SysFunc(string identifier, List<Expression> args, Scope scope)
            : base(identifier, args, scope) { }

        public override string ToString ()
        {
            string str = "";

            if (((prefix is Integer) && (prefix as Integer).value != 1) || ((prefix is Rational) && (prefix as Rational).value.value != 1) || ((prefix is Irrational) && (prefix as Irrational).value != 1))
            {
                str += prefix.ToString() + identifier + '[';
            }
            else
            {
                str += identifier + '[';
            }

            for (int i = 0; i < validArgs.Count; i++) 
            {
                str += validArgs[i].ToString ();

                if (i < validArgs.Count - 1) 
                {
                    str += ',';
                }
            }

            str += ']';

            if (((exponent is Integer) && (exponent as Integer).value != 1) || ((exponent is Rational) && (exponent as Rational).value.value != 1) || ((exponent is Irrational) && (exponent as Irrational).value != 1))
            {
                str += '^' + exponent.ToString();
            }

            return str;
        }

        public bool isArgsValid()
        {
            if (args.Count != validArgs.Count)
                return false;

            for (int i = 0; i < args.Count; i++)
            {
                switch (validArgs[i])
                {
                    case ArgKind.Expression:
                        if (!(args[i] is Expression))
                            return false;
                        break;
                    case ArgKind.Number:
                        if (!(args[i] is Number))
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

