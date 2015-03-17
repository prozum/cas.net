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
			string res;

			var testString = "(x*y)*f(10-x)-20";

			var testEnum = testString.GetEnumerator ();

			testEnum.MoveNext ();

			res = Ast.Parser.ExtractSubstring (testEnum);
			Assert.AreEqual ("x*y", res);

			testEnum.MoveNext ();
			testEnum.MoveNext ();

			res = Ast.Parser.ExtractSubstring (testEnum);
			Assert.AreEqual ("10-x", res);
		}
	}
}

