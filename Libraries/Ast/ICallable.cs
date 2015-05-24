using System;
using System.Collections.Generic;

namespace Ast
{
    public interface ICallable
    {
        bool IsArgumentsValid(List args);

        Expression Call(List args);
    }
}

