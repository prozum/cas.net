using System;
using Ast;

namespace Ast2LaTeX
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Expression exp = Ast.Parser.Parse ("(x-y)*35-20");
            Console.WriteLine (exp.ToString());
        }
    }
}
