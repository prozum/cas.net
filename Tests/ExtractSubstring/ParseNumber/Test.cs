using NUnit.Framework;
using System;
using Ast;

namespace ParseNumber
{
	[TestFixture ()]
	public class ParseNumberTest
	{
		[Test ()]
		public void ParseNumber ()
		{	
			var testString = "12.12.12";
			var test = new Irrational (12.12M);

			var testEnum = testString.GetEnumerator ();

			testEnum.MoveNext ();

			var res = Ast.Parser.ParseNumber (testEnum);
			Assert.AreEqual (test.GetType(), res.GetType());
			Assert.AreEqual (12.12M, (res as Irrational).value);
		}
	}
}


