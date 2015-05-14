using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class FuncStmt :Statement
    {
        public string Identifier;
        public List<Expression> Arguments;
        public List<ArgKind> ValidArguments;

        public FuncStmt(string identifier, List<Expression> args, Scope scope) : base(scope) 
        {
            Identifier = identifier;
            Arguments = args;
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
                        if (!(Arguments[i] is Expression))
                            return false;
                        break;
                    case ArgKind.Number:
                        if (!(Arguments[i] is Real))
                            return false;
                        break;
                    case ArgKind.Symbol:
                        if (!(Arguments[i] is Symbol))
                            return false;
                        break;
                    case ArgKind.Function:
                        if (!(Arguments[i] is Func))
                            return false;
                        break;
                    case ArgKind.Equation:
                        if (!(Arguments[i] is Equal))
                            return false;
                        break;
                    case ArgKind.List:
                        if (!(Arguments[i] is List))
                            return false;
                        break;
                }
            }

            return true;
        }
    }
}

