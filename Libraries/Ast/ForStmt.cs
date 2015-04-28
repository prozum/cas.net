using System;

namespace Ast
{
    public class ForStmt : Statement
    {
        public Symbol sym;
        public Expression list;

        public Scope scope;

        public ForStmt()
        {
        }
    }
}

