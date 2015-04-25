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
            while (tok.kind != TokenKind.EndOfString)
            {
                tokens.Enqueue(tok);
                tok = ScanNext ();
            }

            tokens.Enqueue(new Token(TokenKind.EndOfString, "EndOfString", pos));

            return tokens;
        }

        private static Token ScanNext()
        {
            SkipWhitespace();

            var @char = CharNext();

            switch (@char)
            {
                case EOS:
                    return new Token(TokenKind.EndOfString, @char, pos);
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
                    return new Token(TokenKind.ParentStart, @char, pos);
                case ')':
                    return new Token(TokenKind.ParentEnd, @char, pos);
                case '[':
                    return new Token(TokenKind.SquareStart, @char, pos);
                case ']':
                    return new Token(TokenKind.SquareEnd, @char, pos);
                case '{':
                    return new Token(TokenKind.CurlyStart, @char, pos);
                case '}':
                    return new Token(TokenKind.CurlyEnd, @char, pos);
                case ',':
                    return new Token(TokenKind.Comma, @char, pos);
                case ';':
                    return new Token(TokenKind.Semicolon, @char, pos);
                case '.':
                    return new Token(TokenKind.Dot, @char, pos);
                default:
                    if (char.IsLetter(@char))
                        return ScanIdentifier(@char);
                    else
                        return new Token(TokenKind.Unknown, @char, pos);
            }
        }

        private static Token ScanNumber(char @char)
        {
            TokenKind kind = TokenKind.Integer;
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
                    Errors |= kind == TokenKind.Decimal;
                    kind = TokenKind.Decimal;
                }

                cur = CharNext(false);
            }

            if (cur == 'i')
            {
                kind = kind == TokenKind.Integer ? TokenKind.ImaginaryInt : TokenKind.ImaginaryDec;
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
                    return new Token(TokenKind.KW_True, identifier, startPos);
                case "false":
                    return new Token(TokenKind.KW_False, identifier, startPos);
                default:
                    return new Token(TokenKind.Identifier, identifier, startPos);
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
                    return new Token(TokenKind.Assign, op, startPos);
                case "=":
                    return new Token(TokenKind.Equal, op, startPos);
                case "==":
                    return new Token(TokenKind.BooleanEqual, op, startPos);
                case "<=":
                    return new Token(TokenKind.LesserEqual, op, startPos);
                case ">=":
                    return new Token(TokenKind.GreaterEqual, op, startPos);
                case "<":
                    return new Token(TokenKind.Lesser, op, startPos);
                case ">":
                    return new Token(TokenKind.Greater, op, startPos);
                case "+":
                    return new Token(TokenKind.Add, op, startPos);
                case "-":
                    return new Token(TokenKind.Sub, op, startPos);
                case "*":
                    return new Token(TokenKind.Mul, op, startPos);
                case "/":
                    return new Token(TokenKind.Div, op, startPos);
                case "^":
                    return new Token(TokenKind.Exp, op, startPos);
                default:
                    Errors = true;
                    return new Token(TokenKind.Unknown, op, startPos);
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

