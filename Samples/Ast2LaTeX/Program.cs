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
            if (ex is BinaryOperator) {
                BinaryOperator op = (BinaryOperator)ex;
                return AstLatex (op.Left) + op.sym + AstLatex (op.Right);
            } else if (ex is Symbol) {
                return (ex as Symbol).identifier;
            } else if (ex is Real) {
                return ReturnNumberValue (ex as Real);
            } else {
                return "";
            }       
        }

        public static string ReturnNumberValue (Real num)
        {
            if (num is Int64) {
                return (num as Int64).@int.ToString ();
            } else if (num is Rational) {
                Rational rat = (Rational)num;
                return rat.num.@int.ToString () + "/" + rat.denominator.@int.ToString ();
            } else {
                return (num as Irrational).@decimal.ToString ();
            }
        }
    }
}
