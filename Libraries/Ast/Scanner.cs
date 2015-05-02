using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Ast
{
    public static class Scanner
    {
        const char EOS = (char)0;

        static char[] chars;
        static Pos pos;


        static Error _error = null; 

        public static char CharNext(bool consume = true)
        {
            if (pos.i < chars.Length)
            {
                if (consume)
                {
                    pos.Column++;
                    return chars[pos.i++];
                }
                return chars[pos.i];
            }
            else
                return EOS;
        }

        public static Queue<Token> Tokenize(string tokenString, out Error error)
        {
            var res = new Queue<Token> ();

            chars = tokenString.ToCharArray();
            pos = new Pos();
            error = null;
            Token tok;

            do
            {
                tok = ScanNext();

                if (_error != null)
                {
                    error = _error;
                    _error = null;
                    return null;
                }

                res.Enqueue(tok);
            }
            while (tok.kind != TokenKind.END_OF_STRING);

            return res;
        }

        private static Token ScanNext()
        {
            SkipWhitespace();

            char @char = CharNext();

            switch (@char)
            {
                case EOS:
                    return new Token(TokenKind.END_OF_STRING, "END_OF_STRING", pos);
                
                case '\n':
                    pos.Line++;
                    pos.Column = 0;
                    return new Token(TokenKind.NEW_LINE, "NEW_LINE", pos);

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
                
                case '+':
                    return new Token(TokenKind.ADD, "+", pos);
                case '-':
                    return new Token(TokenKind.SUB, "-", pos);
                case '*':
                    return new Token(TokenKind.MUL, "*", pos);
                case '/':
                    return new Token(TokenKind.DIV, "/", pos);
                case '^':
                    return new Token(TokenKind.EXP, "^", pos);
                case '=':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.BOOL_EQUAL, "==", pos);
                    }
                    else
                        return new Token(TokenKind.EQUAL, "=", pos);
                case '<':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.LESS_EQUAL, "<=", pos);
                    }
                    else
                        return new Token(TokenKind.LESS, "<", pos);
                case '>':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.GREAT_EQUAL, ">=", pos);
                    }
                    else
                        return new Token(TokenKind.GREAT, ">", pos);
                case ':':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.ASSIGN, ":=", pos);
                    }
                    else
                        return new Token(TokenKind.COLON, ":", pos);
                case '!':
                    if (CharNext(false) == '=')
                    {
                        CharNext(true);
                        return new Token(TokenKind.NOT_EQUAL, "!=", pos);
                    }
                    else
                        return new Token(TokenKind.NEG, "!", pos);

                case '(':
                    return new Token(TokenKind.PARENT_START, "(", pos);
                case ')':
                    return new Token(TokenKind.PARENT_END, ")", pos);
                case '[':
                    return new Token(TokenKind.SQUARE_START, "[", pos);
                case ']':
                    return new Token(TokenKind.SQUARE_END, "]", pos);
                case '{':
                    return new Token(TokenKind.CURLY_START, "{", pos);
                case '}':
                    return new Token(TokenKind.CURLY_END, "}", pos);
                case ',':
                    return new Token(TokenKind.COMMA, ",", pos);
                case ';':
                    return new Token(TokenKind.SEMICOLON, ";", pos);
                case '.':
                    return new Token(TokenKind.DOT, ".", pos);
                
                default:
                    if (char.IsLetter(@char))
                        return ScanIdentifier(@char);
                    else
                        return new Token(TokenKind.UNKNOWN, @char, pos);
            }
        }

        private static Token ScanNumber(char @char)
        {
            TokenKind kind = TokenKind.INTEGER;
            string number = @char.ToString();
            Pos startPos = pos;


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
                        ReportSyntaxError("Decimal with more than one seperator");
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
                        ReportSyntaxError("Missing end of string");
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
            var startPos = pos;
            return new Token(TokenKind.TEXT, ExtractSubText(@char), startPos);
        }

        private static Token ScanIdentifier(char @char)
        {
            string identifier = @char.ToString();
            Pos startPos = pos;

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
                case "if":
                    return new Token(TokenKind.IF, identifier, startPos);
                case "elif":
                    return new Token(TokenKind.ELIF, identifier, startPos);
                case "else":
                    return new Token(TokenKind.ELSE, identifier, startPos);
                case "return":
                    return new Token(TokenKind.RETURN, identifier, startPos);
                case "for":
                    return new Token(TokenKind.FOR, identifier, startPos);
                case "in":
                    return new Token(TokenKind.IN, identifier, startPos);
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

        public static Error ReportSyntaxError(string msg)
        {
            _error = new Error("Scanner: " + msg);
            _error.pos = pos;

            return _error;
        }
    }
}

