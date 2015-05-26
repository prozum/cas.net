using System;

namespace Ast
{
    public enum TokenKind
    {
        IDENTIFIER,
        TRUE,
        FALSE,
        NULL,
        SELF,

        IF,
        ELIF,
        ELSE,
        FOR,
        IN,
        WHILE,
        RET,
        IMPORT,

        TEXT,

        INTEGER,
        DECIMAL,
        IMAG_INT,
        IMAG_DEC,

        TILDE,
        ASSIGN,
        EQUAL,
        BOOL_EQUAL,
        LESS_EQUAL,
        GREAT_EQUAL,
        NOT_EQUAL,
        LESS,
        GREAT,
        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        EXP,
        NEG,
        AND,
        OR,
        
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
        HASH,

        NEW_LINE,
        END_OF_STRING,
        NONE,
        UNKNOWN
    }

    public struct Pos
    {
        public int i;

        public int Line;
        public int Column;

        //public Pos()
        //{
        //    this.i = 0;
        //    Line = 1;
        //    Column = 0;
        //}
        public Pos(int i = 0, int line = 1, int column = 0)
        {
            this.i = i;
            Line = line;
            Column = column;
        }
    }

    public class Token
    {
        public TokenKind Kind;
        public string Value;
        public Pos Position;

        public Token(TokenKind kind, char value, Pos pos) : this(kind, value.ToString(), pos) {}
        public Token(TokenKind kind, string value, Pos pos)
        {
            this.Kind = kind;
            this.Value = value;
            this.Position = pos;
            this.Position.i++;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

