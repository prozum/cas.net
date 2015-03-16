using NUnit.Framework;
using System;
using Ast;

namespace ExtractSubstring
{
	[TestFixture]
	public class ExtractSubstringTest
	{
		[Test]
		public void ExtractSubstring()
		{
			var testString = "(x*y)*f(10-x)";

			var testEnum = testString.GetEnumerator ();

			//testEnum.MoveNext ();

			var res = Ast.Parser.ExtractSubstring (testEnum, 'v');

			Assert.AreEqual ("x*y", res);
		}
	}
}

