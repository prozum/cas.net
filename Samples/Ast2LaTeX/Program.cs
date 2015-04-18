using System;
using Ast;

namespace Ast2LaTeX
{
    class MainClass
    {
        public static void Main (string[] args)
        {
			var parser = new Parser ();
            Expression exp = parser.Parse ("(x-y)*35-20");
            Console.WriteLine (AstLatex (exp));
        }

        public static string AstLatex (Expression ex)
        {
            if (ex is Operator) {
                Operator op = (Operator)ex;
                return AstLatex (op.left) + op.symbol + AstLatex (op.right);
            } else if (ex is Symbol) {
                return (ex as Symbol).identifier;
            } else if (ex is Number) {
                return ReturnNumberValue (ex as Number);
            } else {
                return "";
            }       
        }

        public static string ReturnNumberValue (Number num)
        {
            if (num is Integer) {
                return (num as Integer).value.ToString ();
            } else if (num is Rational) {
                Rational rat = (Rational)num;
                return rat.numerator.value.ToString () + "/" + rat.denominator.value.ToString ();
            } else {
                return (num as Irrational).value.ToString ();
            }
        }
    }
}
