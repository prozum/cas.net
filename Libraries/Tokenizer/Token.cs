using System;

namespace Ast
{
    public enum TokenKind
    {
        Identifier,
        Number,
        Operator,
        Bracket
    }

    public class Token
    {
        public string value;
        public TokenKind kind;
        public int column;

        public Token(string value, TokenKind kind, int column)
        {
            this.value = value;
            this.kind = kind;
            this.column = column;
        }
    }
}

