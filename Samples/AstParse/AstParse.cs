using System;
using Ast;

class AstParse
{
	public static void Main (string[] args)
	{
		var exp = Ast.Parser.Parse ("(x-y)*f(10-x,33)-20");
		Console.WriteLine (exp.ToString());
	}
}

