using System;

namespace Ast
{
    public enum TokenKind
    {
        Identifier,
        KW_True,
        KW_False,

        Integer,
        Decimal,
        ImaginaryInt,
        ImaginaryDec,

        Assign,
        Equal,
        BooleanEqual,
        LesserOrEqual,
        GreaterOrEqual,
        Lesser,
        Greater,
        Add,
        Sub,
        Mul,
        Div,
        Exp,
        
        ParenthesesStart,
        ParenthesesEnd,
        SquareStart,
        SquareEnd,
        CurlyStart,
        CurlyEnd,

        Comma,
        Semicolon,

        EndOfString,
        None,
        Unknown
    }

    public class Token
    {
        public TokenKind kind;
        public string value;
        public int pos;

        public Token(TokenKind kind, string value, int pos)
        {
            this.kind = kind;
            this.value = value;
            this.pos = pos + 1;
        }
    }
}

