using System;
using Ast;

class AstParse
{
	public static void Main (string[] args)
	{
		var exp = Ast.Parser.Parse ("x*(y-20)*50");
		Console.WriteLine (exp.ToString());
	}
}

