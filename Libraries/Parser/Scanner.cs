using System;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace Parser
{
    public class Scanner
    {
        const char EOS = (char)0;
        readonly char sep = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator[0];
        readonly char[] opChars = {'=', '<', '>', '+', '-', '*', '/', '^', ':'};

        private bool Errors = false; 

        private string tokenString;
        private char[] chars;
        private int pos;
        private char cur
        {
            get
            {
                if (pos < chars.Length)
                    return chars[pos];
                else
                    return EOS;
            }
        }

        public Scanner ()
        {
        }

        public Queue<Token> Tokenize(string tokenString)
        {
            this.tokenString = tokenString;
            this.chars = tokenString.ToCharArray();

            var tokens = new Queue<Token> ();

            var tok = ScanNext();
            while (tok.kind != TokenKind.EndOfString)
            {
                tokens.Enqueue(tok);
                tok = ScanNext ();
            }

            return tokens;
        }

        private Token ScanNext()
        {
            SkipWhitespace();

            switch (cur)
            {
                case EOS:
                    return new Token(TokenKind.EndOfString, cur.ToString(), pos);
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
                    return ScanNumber();
                case '=':
                case '<':
                case '>':
                case '+':
                case '-':
                case '*':
                case '/':
                case '^':
                case ':':
                    return ScanOperator();
                case '(':
                    return new Token(TokenKind.ParenthesesStart, cur.ToString(), pos);
                case ')':
                    return new Token(TokenKind.ParenthesesEnd, cur.ToString(), pos);
                case '[':
                    return new Token(TokenKind.SquareStart, cur.ToString(), pos);
                case ']':
                    return new Token(TokenKind.SquareEnd, cur.ToString(), pos);
                case '{':
                    return new Token(TokenKind.CurlyStart, cur.ToString(), pos);
                case '}':
                    return new Token(TokenKind.CurlyEnd, cur.ToString(), pos);
                case ',':
                    return new Token(TokenKind.Comma, cur.ToString(), pos);
                case ';':
                    return new Token(TokenKind.Semicolon, cur.ToString(), pos);
                default:
                    if (char.IsLetter(cur))
                        return ScanIdentifier();
                    else
                        return new Token(TokenKind.Unknown, cur.ToString(), pos);
            }
        }

        private Token ScanNumber()
        {
            TokenKind kind = TokenKind.Integer;
            string number = "";
            int startPos = pos;

            while (char.IsDigit(cur) || cur == sep)
            {
                number += cur;

                if (cur == sep)
                {
                    //More than one Seperator. Error!
                    if (kind == TokenKind.Decimal )
                    {
                        Errors = true;
                    }
                    kind = TokenKind.Decimal;
                }
                pos++;
            }

            if (cur == 'i')
            {
                if (kind == TokenKind.Integer)
                    kind = TokenKind.ImaginaryInt;
                else
                    kind = TokenKind.ImaginaryDec;
                pos++;
            }

            return new Token(kind, number, startPos);
        }

        private Token ScanIdentifier()
        {
            string identifier = "";
            int startPos = pos;

            while (char.IsLetterOrDigit (cur))
            {
                identifier += cur;
                pos++;
            }

            return new Token(TokenKind.Identifier, identifier, startPos);
        }

        private Token ScanOperator()
        {
            string op = cur.ToString();
            int startPos = pos;
            pos++;

            if (opChars.Contains(cur))
            {
                op += cur;
                pos++;
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
                    return new Token(TokenKind.LesserOrEqual, op, startPos);
                case ">=":
                    return new Token(TokenKind.GreaterOrEqual, op, startPos);
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

        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace (cur)) 
            {
                pos++;
            }
        }
    }
}

