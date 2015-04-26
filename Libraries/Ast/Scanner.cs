using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Ast
{
    public static class Scanner
    {
        const char EOS = (char)0;
        static readonly char sep = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator[0];
        static readonly char[] opChars = {'=', '<', '>', '+', '-', '*', '/', '^', ':'};

        static bool Errors = false; 

        static string tokenString;
        static char[] chars;
        static Pos pos;

//        static char cur
//        {
//            get
//            {
//                if (pos.i < chars.Length)
//                    return chars[pos.i++];
//                else
//                    return EOS;
//            }
//        }
//        static char cur
//        {
//            get
//            {
//                if (pos.i < chars.Length)
//                    return chars[pos.i];
//                else
//                    return EOS;
//            }
//        }

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

        public static Queue<Token> Tokenize(string str)
        {
            tokenString = str;
            chars = tokenString.ToCharArray();
            pos = new Pos();

            var tokens = new Queue<Token> ();

            var tok = ScanNext();
            while (tok.kind != TokenKind.END_OF_STRING)
            {
                tokens.Enqueue(tok);
                tok = ScanNext ();
            }

            tokens.Enqueue(new Token(TokenKind.END_OF_STRING, "EndOfString", pos));

            return tokens;
        }

        private static Token ScanNext()
        {
            SkipWhitespace();

            var @char = CharNext();

            switch (@char)
            {
                case EOS:
                    return new Token(TokenKind.END_OF_STRING, @char, pos);
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
                case '=':
                case '<':
                case '>':
                case '+':
                case '-':
                case '*':
                case '/':
                case '^':
                case ':':
                    return ScanOperator(@char);
                case '(':
                    return new Token(TokenKind.PARENT_START, @char, pos);
                case ')':
                    return new Token(TokenKind.PARENT_END, @char, pos);
                case '[':
                    return new Token(TokenKind.SQUARE_START, @char, pos);
                case ']':
                    return new Token(TokenKind.SQUARE_END, @char, pos);
                case '{':
                    return new Token(TokenKind.CURLY_START, @char, pos);
                case '}':
                    return new Token(TokenKind.CURLY_END, @char, pos);
                case ',':
                    return new Token(TokenKind.COMMA, @char, pos);
                case ';':
                    return new Token(TokenKind.SEMICOLON, @char, pos);
                case '.':
                    return new Token(TokenKind.DOT, @char, pos);
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
            while (char.IsDigit(cur) || cur == sep)
            {
                CharNext();
                number += cur;

                if (cur == sep)
                {
                    //More than one Seperator. Error!
                    Errors |= kind == TokenKind.DECIMAL;
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
                case "else":
                    return new Token(TokenKind.ELSE, identifier, startPos);
                case "return":
                    return new Token(TokenKind.RETURN, identifier, startPos);
                default:
                    return new Token(TokenKind.IDENTIFIER, identifier, startPos);
            }
        }

        private static Token ScanOperator(char @char)
        {
            string op = @char.ToString();
            Pos startPos = pos;

            var cur = CharNext(false);
            if (opChars.Contains(cur))
            {
                op += cur;
                CharNext();
            }

            switch(op)
            {
                case ":=":
                    return new Token(TokenKind.ASSIGN, op, startPos);
                case "=":
                    return new Token(TokenKind.EQUAL, op, startPos);
                case "==":
                    return new Token(TokenKind.BOOL_EQUAL, op, startPos);
                case "<=":
                    return new Token(TokenKind.LESS_EQUAL, op, startPos);
                case ">=":
                    return new Token(TokenKind.GREAT_EQUAL, op, startPos);
                case "<":
                    return new Token(TokenKind.LESS, op, startPos);
                case ">":
                    return new Token(TokenKind.GREAT, op, startPos);
                case "+":
                    return new Token(TokenKind.ADD, op, startPos);
                case "-":
                    return new Token(TokenKind.SUB, op, startPos);
                case "*":
                    return new Token(TokenKind.MUL, op, startPos);
                case "/":
                    return new Token(TokenKind.DIV, op, startPos);
                case "^":
                    return new Token(TokenKind.EXP, op, startPos);
                default:
                    Errors = true;
                    return new Token(TokenKind.UNKNOWN, op, startPos);
            }
        }

        private static void SkipWhitespace()
        {
            var cur = CharNext(false);
            while (char.IsWhiteSpace (cur) || cur == '\n') 
            {
                CharNext(true);
                if (cur == '\n')
                {
                    pos.Line++;
                    pos.Column = 0;
                }
                cur = CharNext(false);
            }
        }
    }
}

