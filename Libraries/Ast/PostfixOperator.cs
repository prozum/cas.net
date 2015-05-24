using System;

namespace Ast
{
    public class PostfixOperator : Expression
    {
        public Expression Child;

        public PostfixOperator()
        {
        }
    }
}

