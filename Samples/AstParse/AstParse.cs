using System;
using Ast;

class AstParse
{
	public static void Main (string[] args)
	{
		var exp = Ast.Parser.Parse ("x^(y-20)-earthmass");
		Console.WriteLine (exp.ToString());
	}
}

