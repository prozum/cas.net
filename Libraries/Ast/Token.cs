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
        LesserEqual,
        GreaterEqual,
        Lesser,
        Greater,
        Add,
        Sub,
        Mul,
        Div,
        Exp,
        
        ParentStart,
        ParentEnd,
        SquareStart,
        SquareEnd,
        CurlyStart,
        CurlyEnd,

        Comma,
        Semicolon,
        Dot,

        EndOfString,
        None,
        Unknown
    }

    public struct Pos
    {
        public int i;

        public int Line;
        public int Column;

        public Pos()
        {
            i = 0;
            Line = 1;
            Column = 0;
        }
    }

    public class Token
    {
        public TokenKind kind;
        public string value;
        public Pos pos;

        public Token(TokenKind kind, char value, Pos pos) : this(kind, value.ToString(), pos) {}
        public Token(TokenKind kind, string value, Pos pos)
        {
            this.kind = kind;
            this.value = value;
            this.pos = pos;
            this.pos.i++;
        }

        public override string ToString()
        {
            return value;
        }
    }
}

