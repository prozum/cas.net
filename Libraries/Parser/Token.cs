﻿using System;

namespace Parser
{
    public enum TokenKind
    {
        EndOfString,
        Identifier,

        Integer,
        Decimal,
        Imaginary,

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
            this.pos = pos;
        }
    }
}

