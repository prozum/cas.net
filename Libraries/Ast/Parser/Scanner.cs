using System.Collections.Generic;

namespace Ast
{
    public static class Scanner
    {
        const char EOS = (char)0;

        static char[] Chars;
        static Pos Position;

        static List<Error> Errors; 

        public static char CharNext(bool consume = true)
        {
            if (Position.i < Chars.Length)
            {
                if (consume)
                {
                    Position.Column++;
                    return Chars[Position.i++];
                }
                return Chars[Position.i];
            }
            else
                return EOS;
        }

        public static Queue<Token> Tokenize(string tokenString, List<Error> errors)
        {
            var res = new Queue<Token> ();

            Chars = tokenString.ToCharArray();
            Position = new Pos();
            Token tok;

            Errors = errors;

            do
            {
                tok = ScanNext();

                if (errors.Count > 0)
                    return null;

                res.Enqueue(tok);
            }
            while (tok.Kind != TokenKind.END_OF_STRING);

            return res;
        }

        private static Token ScanNext()
        {
            SkipWhitespace();

            char @char = CharNext();

            switch (@char)
            {
                case EOS:
                    return new Token(TokenKind.END_OF_STRING, "END_OF_STRING", Position);
                
                case '\n':
                    Position.Line++;
                    Position.Column = 0;
                    return new Token(TokenKind.NEW_LINE, "NEW_LINE", Position);

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ScanNumber(@char);
                
                case '"':
                case '\'':
                    return ScanText(@char);
                
                case '~':
                    return new Token(TokenKind.TILDE, "~", Position);
                case '+':
                    return new Token(TokenKind.ADD, "+", Position);
                case '-':
                    return new Token(TokenKind.SUB, "-", Position);
                case '*':
                    return new Token(TokenKind.MUL, "*", Position);
                case '/':
                    return new Token(TokenKind.DIV, "/", Position);
                case '%':
                    return new Token(TokenKind.MOD, "%", Position);
                case '^':
                    return new Token(TokenKind.EXP, "^", Position);
                case '&':
                    return new Token(TokenKind.AND, "&", Position);
                case '|':
                    return new Token(TokenKind.OR, "|", Position);
                case '=':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.BOOL_EQUAL, "==", Position);
                    }
                    else
                        return new Token(TokenKind.EQUAL, "=", Position);
                case '<':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.LESS_EQUAL, "<=", Position);
                    }
                    else
                        return new Token(TokenKind.LESS, "<", Position);
                case '>':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.GREAT_EQUAL, ">=", Position);
                    }
                    else
                        return new Token(TokenKind.GREAT, ">", Position);
                case ':':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.ASSIGN, ":=", Position);
                    }
                    else
                        return new Token(TokenKind.COLON, ":", Position);
                case '!':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.NOT_EQUAL, "!=", Position);
                    }
                    else
                        return new Token(TokenKind.NEG, "!", Position);

                case '(':
                    return new Token(TokenKind.PARENT_START, "(", Position);
                case ')':
                    return new Token(TokenKind.PARENT_END, ")", Position);
                case '[':
                    return new Token(TokenKind.SQUARE_START, "[", Position);
                case ']':
                    return new Token(TokenKind.SQUARE_END, "]", Position);
                case '{':
                    return new Token(TokenKind.CURLY_START, "{", Position);
                case '}':
                    return new Token(TokenKind.CURLY_END, "}", Position);
                
                case ',':
                    return new Token(TokenKind.COMMA, ",", Position);
                case ';':
                    return new Token(TokenKind.SEMICOLON, ";", Position);
                case '.':
                    return new Token(TokenKind.DOT, ".", Position);
                case '#':
                    return new Token(TokenKind.HASH, "#", Position);
                
                default:
                    if (char.IsLetter(@char))
                        return ScanIdentifier(@char);
                    else
                        return new Token(TokenKind.UNKNOWN, @char, Position);
            }
        }

        private static Token ScanNumber(char @char)
        {
            TokenKind kind = TokenKind.INTEGER;
            string number = @char.ToString();
            Pos startPos = Position;


            var cur = CharNext(false);
            while (char.IsDigit(cur) || cur == '.')
            {
                CharNext();
                number += cur;

                if (cur == '.')
                {
                    //More than one Seperator. Error!
                    if (kind == TokenKind.DECIMAL)
                    {
                        ReportError("Decimal with more than one seperator");
                        return null;
                    }
                    kind = TokenKind.DECIMAL;
                }

                cur = CharNext(false);
            }

            if (cur == 'i')
            {
                kind = kind == TokenKind.INTEGER ? TokenKind.IMAG_INT : TokenKind.IMAG_DEC;
                CharNext();
            }

            return new Token(kind, number, startPos);
        }
        enum TextContext
        {

        }

        private static string ExtractSubText(char endChar)
        {
            string res = "";

            char subChar;
            if (endChar == '"')
                subChar = '\'';
            else
                subChar = '"';
                
            do
            {
                var cur = CharNext(true);
                switch (cur)
                {
                    case '"':
                    case '\'':
                        if (cur == endChar)
                            return res;
                        else
                            res += subChar + ExtractSubText(cur) + subChar;
                        break;
                    case EOS:
                        ReportError("Missing end of string");
                        return "";
                    default:
                        res += cur;
                        break;
                }
            }
            while (true);
        }

        private static Token ScanText(char @char)
        {
            var startPos = Position;
            return new Token(TokenKind.TEXT, ExtractSubText(@char), startPos);
        }

        private static Token ScanIdentifier(char @char)
        {
            string identifier = @char.ToString();
            Pos startPos = Position;

            var cur = CharNext(false);
            while (char.IsLetterOrDigit (cur))
            {
                CharNext(true);
                identifier += cur;
                cur = CharNext(false);
            }

            identifier = identifier.ToLower();

            switch (identifier)
            {
                case "true":
                    return new Token(TokenKind.TRUE, identifier, startPos);
                case "false":
                    return new Token(TokenKind.FALSE, identifier, startPos);
                case "null":
                    return new Token(TokenKind.NULL, identifier, startPos);
                case "if":
                    return new Token(TokenKind.IF, identifier, startPos);
                case "elif":
                    return new Token(TokenKind.ELIF, identifier, startPos);
                case "else":
                    return new Token(TokenKind.ELSE, identifier, startPos);
                case "for":
                    return new Token(TokenKind.FOR, identifier, startPos);
                case "in":
                    return new Token(TokenKind.IN, identifier, startPos);
                case "while":
                    return new Token(TokenKind.WHILE, identifier, startPos);
                case "ret":
                    return new Token(TokenKind.RET, identifier, startPos);
                case "import":
                    return new Token(TokenKind.IMPORT, identifier, startPos);
                case "and":
                    return new Token(TokenKind.AND, identifier, startPos);
                case "or":
                    return new Token(TokenKind.OR, identifier, startPos);
                case "self":
                    return new Token(TokenKind.SELF, identifier, startPos);
                default:
                    return new Token(TokenKind.IDENTIFIER, identifier, startPos);
            }
        }

        private static void SkipWhitespace()
        {
            while (char.IsWhiteSpace (CharNext(false)) && CharNext(false) != '\n') 
            {
                CharNext(true);
            }
        }

        public static void ReportError(string msg)
        {
            var error = new Error("Scanner: " + msg);
            error.Position = Position;
            Errors.Add(error);
        }
    }
}

