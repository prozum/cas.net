using System;

namespace Ast
{
    public class Null : Expression
    {
        public Null()
        {
        }

        public override string ToString()
        {
            return "null";
        }
    }
}

