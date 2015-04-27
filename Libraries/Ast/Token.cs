using System;

namespace Ast
{
    public enum TokenKind
    {
        IDENTIFIER,
        TRUE,
        FALSE,
        IF,
        ELIF,
        ELSE,
        RETURN,

        TEXT,

        INTEGER,
        DECIMAL,
        IMAG_INT,
        IMAG_DEC,

        ASSIGN,
        EQUAL,
        BOOL_EQUAL,
        LESS_EQUAL,
        GREAT_EQUAL,
        LESS,
        GREAT,
        ADD,
        SUB,
        MUL,
        DIV,
        EXP,
        
        PARENT_START,
        PARENT_END,
        SQUARE_START,
        SQUARE_END,
        CURLY_START,
        CURLY_END,

        COMMA,
        COLON,
        SEMICOLON,
        DOT,

        END_OF_STRING,
        NONE,
        UNKNOWN
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

